using Cartif.Extensions;
using Cartif.Logs;
using Cartif.XamlResources;
using GenericForms.Abstract;
using GenericForms.Settings;
using LAE.Biomasa.Modelo;
using LAE.Biomasa.Pages;
using LAE.Biomasa.Ventanas;
using LAE.Comun.Modelo;
using LAE.Comun.Modelo.Procedimientos;
using LAE.Comun.Persistence;
using LAE.Modelo;
using MahApps.Metro.Controls;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI.Pages
{
    /// <summary>
    /// Lógica de interacción para Ensayos.xaml
    /// </summary>
    public partial class Ensayos : UserControl
    {
        private bool cargar = false;
        private List<EnsayoPNT> ListaEnsayos;
        private Tecnico[] Tecnicos;
        private Equipo[] Equipos;

        public Ensayos()
        {
            InitializeComponent();
        }

        private void pageEnsayos_Loaded(object sender, RoutedEventArgs e)
        {
            if (cargar)
            {
                cmbEquipos.ItemsSource = PersistenceManager.SelectAll<Equipo>().Where(eq => eq.EquipoEnsayo == true).ToArray();
                CargarDatos();
                GenerarGrid();
            }
            else
                cargar = true;
        }

        private void CargarDatos()
        {
            ListaEnsayos = PersistenceManager.SelectAll<EnsayoPNT>().OrderByDescending(en => en.FechaInicio).ToList();
            Tecnicos = PersistenceManager.SelectAll<Tecnico>().ToArray();
            Equipos = PersistenceManager.SelectAll<Equipo>().ToArray();
        }

        private void GenerarGrid()
        {
            gridEnsayos.Build<EnsayoPNT>(new TypeGridSettings()
            {
                Columns = new ColumnGridSettings
                {
                    ["IdEquipo"] = new TypeGridColumnSettings
                    {
                        Label = "Equipo",
                        ColumnCombo = new TypeGCComboSettings
                        {
                            InnerValues = Equipos,
                            Path = "Id",
                        }
                    },
                    ["FechaInicio"] = TypeGridColumnSettingsEnum.DefaultColum
                        .SetLabel("Fecha inicio")
                        .SetFormat("dd/MM/yy"),
                    ["Editar"] = new TypeGridColumnSettings
                    {
                        Label = "Editar/Ver",
                        ColumnButton = new TypeGCButtonSettings
                        {
                            DesingPath = CSVPath.EditIcon,
                            Color = Colors.White,
                            Size = 25,
                            Margin = 4,
                            Click = (sender, e) =>
                            {
                                EnsayoPNT ensayo = gridEnsayos.SelectedItem.Clone(typeof(EnsayoPNT)) as EnsayoPNT;
                                MetroWindow ventana = null;

                                if (FactoriaEquipos.GetEquipoByTipo("Analizador elemental").Any(eq => eq.Id == ensayo.IdEquipo))
                                    ventana = new WindowEquipoCHN() { Ensayo = ensayo };
                                else if (FactoriaEquipos.GetEquipoByTipo("Analizador fusibilidad").Any(eq => eq.Id == ensayo.IdEquipo))
                                    ventana = new WindowEquipoFus() { Ensayo = ensayo };

                                if (ventana != null)
                                {
                                    ventana.ShowDialog();

                                    ListaEnsayos = PersistenceManager.SelectAll<EnsayoPNT>().OrderByDescending(en => en.FechaInicio).ToList();
                                    gridEnsayos.FillDataGrid(ListaEnsayos);
                                }
                            }
                        }
                    },
                    ["Borrar"] = new TypeGridColumnSettings
                    {
                        ColumnButton = new TypeGCButtonSettings
                        {
                            DesingPath = CSVPath.RemoveIcon,
                            Color = Colors.White,
                            Size = 25,
                            Margin = 4,
                            Click = (sender, e) =>
                            {
                                EnsayoPNT ensayo = ((FrameworkElement)sender).DataContext as EnsayoPNT;

                                if (!PrepararBorrarEnsayo<ReplicaChn, ChnControl>(ensayo, "Analizador elemental", BorrarEnsayo, true))
                                    PrepararBorrarEnsayo<ReplicaFusibilidad, FusibilidadControl>(ensayo, "Analizador fusibilidad", BorrarEnsayo);
                            }
                        }
                    }
                }
            });

            gridEnsayos.FillDataGrid(ListaEnsayos);
        }

        private Boolean PrepararBorrarEnsayo<T, T2>(EnsayoPNT ensayo, String equipo, Action<EnsayoPNT, Boolean> borrado, Boolean chn = false) where T : PersistenceData where T2 : PersistenceData
        {
            if (FactoriaEquipos.GetEquipoByTipo(equipo).Any(eq => eq.Id == ensayo.IdEquipo))
            {
                if (PersistenceManager.SelectByProperty<T>("IdEnsayo", ensayo.Id).Any())
                {
                    MessageBox.Show("No se puede borrar el ensayo, hay réplicas que usan el ensayo.");
                }
                else if (PersistenceManager.SelectByProperty<T2>("IdEnsayo", ensayo.Id).Any())
                {
                    MessageBox.Show("No se puede borrar el ensayo, contiene Controles de Calidad Internos. Borrales previamente antes de borrar el ensayo");
                }
                else
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("¿Estas seguro que desea borrar el ensayo y su deriva?. Una vez eliminada, sus datos desaparecerán definitivamente", "Borrar", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        borrado(ensayo, chn);
                    }
                }

                return true;
            }

            return false;
        }

        private void BorrarEnsayo(EnsayoPNT ensayo, Boolean chn)
        {
            using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
            using (NpgsqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    PersistenceDataManipulation.Borrar1N<MuestraEnsayo>(conn, null, ensayo.Id, "IdEnsayo");
                    PersistenceDataManipulation.Borrar1N<EquipoEnsayo>(conn, null, ensayo.Id, "IdEnsayo");
                    if (chn)
                        BorrarDeriva(conn, ensayo);

                    ensayo.Delete(conn);

                    ListaEnsayos.Remove(ensayo);

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    if (trans != null)
                        trans.Rollback();

                    CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "Error al borra el ensayo", ex);
                    MessageBox.Show("Error al borrar el ensayo. Por favor, informa a soporte.");
                }
            }
        }

        private void BorrarDeriva(NpgsqlConnection conn, EnsayoPNT ensayo)
        {
            ChnDeriva deriva = FactoriaChnDeriva.GetCHNderiva(ensayo.Id);
            if (deriva != null)
            {
                PersistenceDataManipulation.Borrar1N<ReplicaChnDeriva>(conn, null, deriva.Id, "IdCHNderiva");
                deriva.Delete(conn);
            }
        }

        private void nuevoEnsayo_Click(object sender, RoutedEventArgs e)
        {
            Equipo equipo = cmbEquipos.SelectedItem as Equipo;
            if (equipo != null)
            {
                MetroWindow ventana = null;
                EnsayoPNT ensayo = new EnsayoPNT() { IdEquipo = equipo.Id, FechaInicio = DateTime.Now };
                if (FactoriaEquipos.GetEquipoByTipo("Analizador elemental").Any(eq => eq.Id == equipo.Id))
                    ventana = new WindowEquipoCHN { Ensayo = ensayo };
                else if (FactoriaEquipos.GetEquipoByTipo("Analizador fusibilidad").Any(eq => eq.Id == equipo.Id))
                    ventana = new WindowEquipoFus { Ensayo = ensayo };

                if (ventana != null)
                {
                    ventana.ShowDialog();
                    ListaEnsayos = PersistenceManager.SelectAll<EnsayoPNT>().OrderByDescending(en => en.FechaInicio).ToList();
                    gridEnsayos.FillDataGrid(ListaEnsayos);
                }
            }
        }

    }
}
