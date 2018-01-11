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
        private ObservableCollection<EnsayoPNT> ListaEnsayos;
        private Tecnico[] Tecnicos;
        private Equipo[] Equipos;

        public Ensayos()
        {
            InitializeComponent();
            ListaEnsayos = new ObservableCollection<EnsayoPNT>() { new EnsayoPNT() };
        }

        private void pageEnsayos_Loaded(object sender, RoutedEventArgs e)
        {
            if (cargar)
            {
                cmbEquipos.ItemsSource = PersistenceManager.SelectAll<Equipo>().Where(eq => eq.EquipoEnsayo == true).ToArray();
                cmbEquipos.DisplayMemberPath = "Nombre";
                CargarDatos();
                GenerarGrid();
            }
            else
                cargar = true;
        }

        private void GenerarGrid()
        {
            gridEnsayos.Build(ListaEnsayos, new TypeGridSettings()
            {
                Columns = new ColumnGridSettings
                {
                    ["Id"]=TypeGridColumnSettingsEnum.DefaultColum,
                    ["IdEquipo"] = new TypeGridColumnSettings
                    {
                        Label = "Equipo",
                        ColumnCombo = new TypeGCComboSettings
                        {
                            InnerValues = Equipos,
                            Path = "Id",
                            DisplayPath = "Nombre"
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
                                if (FactoriaEquipos.GetEquipoByTipo("Analizador elemental").Any(eq => eq.Id == ensayo.IdEquipo))
                                {
                                    WindowEquipoCHN ventana = new WindowEquipoCHN() { Ensayo = ensayo };
                                    ventana.ShowDialog();
                                    ListaEnsayos = new ObservableCollection<EnsayoPNT>(PersistenceManager.SelectAll<EnsayoPNT>().OrderByDescending(en => en.FechaInicio).ToArray());
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

                                  if (FactoriaEquipos.GetEquipoByTipo("Analizador elemental").Any(eq => eq.Id == ensayo.IdEquipo))
                                  {
                                      if (PersistenceManager.SelectByProperty<ReplicaChn>("IdEnsayo", ensayo.Id).Any())
                                      {
                                          MessageBox.Show("No se puede borrar el ensayo, hay réplicas que usan el ensayo.");
                                      }
                                      else if (PersistenceManager.SelectByProperty<ChnControl>("IdEnsayo", ensayo.Id).Any())
                                      {
                                          MessageBox.Show("No se puede borrar el ensayo, cotiene Controles de Calidad Internos. Borrales previamente antes de borrar el ensayo");
                                      }
                                      else
                                      {
                                          MessageBoxResult messageBoxResult = MessageBox.Show("¿Estas seguro que desea borrar el ensayo y su deriva?. Una vez eliminada, sus datos desaparecerán definitivamente", "Borrar", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                                          if (messageBoxResult == MessageBoxResult.Yes)
                                          {
                                              BorrarEnsayo(ensayo);
                                          }
                                      }
                                  }
                              }
                        }
                    }
                }
            });
        }

        private void CargarDatos()
        {
            ListaEnsayos = new ObservableCollection<EnsayoPNT>(PersistenceManager.SelectAll<EnsayoPNT>().OrderByDescending(en => en.FechaInicio).ToArray());
            Tecnicos = PersistenceManager.SelectAll<Tecnico>().ToArray();
            Equipos = PersistenceManager.SelectAll<Equipo>().ToArray();
        }

        private void BorrarEnsayo(EnsayoPNT ensayo)
        {
            using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
            using (NpgsqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    PersistenceDataManipulation.Borrar1N<MuestraEnsayo>(conn, null, ensayo.Id, "IdEnsayo");
                    ChnDeriva deriva = FactoriaChnDeriva.GetCHNderiva(ensayo.Id);
                    if (deriva != null)
                    {
                        PersistenceDataManipulation.Borrar1N<ReplicaChnDeriva>(conn, null, deriva.Id, "IdCHNderiva");
                        deriva.Delete(conn);
                    }
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

        private void nuevoEnsayo_Click(object sender, RoutedEventArgs e)
        {
            Equipo equipo = cmbEquipos.SelectedItem as Equipo;
            if (equipo != null)
            {
                if (FactoriaEquipos.GetEquipoByTipo("Analizador elemental").Any(eq => eq.Id == equipo.Id))
                {
                    WindowEquipoCHN ventana = new WindowEquipoCHN() { Ensayo = new EnsayoPNT() { IdEquipo = equipo.Id, FechaInicio = DateTime.Now } };
                    ventana.ShowDialog();
                    ListaEnsayos = new ObservableCollection<EnsayoPNT>(PersistenceManager.SelectAll<EnsayoPNT>().OrderByDescending(en => en.FechaInicio).ToArray());
                    gridEnsayos.FillDataGrid(ListaEnsayos);
                }
            }
        }
    }
}
