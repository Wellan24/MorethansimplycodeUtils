using Cartif.Logs;
using Cartif.XamlResources;
using GenericForms.Abstract;
using GenericForms.Settings;
using LAE.Biomasa.Modelo;
using LAE.Comun.Modelo.Procedimientos;
using LAE.Comun.Persistence;
using Npgsql;
using System;
using System.Collections.Generic;
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

namespace LAE.Biomasa.Pages
{
    /// <summary>
    /// Lógica de interacción para PageAnalisisEquipo.xaml
    /// </summary>
    public partial class PageAnalisisEquipo : UserControl
    {
        private Boolean SinHumedades { get; set; }

        public Action<Analisis> BorrarAnalisisCci { get; set; }

        public Func<Analisis[]> RecargarAnalisis { get; set; }

        public Action<Analisis> GuardarOrden { get; set; }

        public Analisis[] ListaAnalisis;
        private MuestraAnalisis[] MuestrasAnalisis;
        private EnsayoPNT Ensayo;

        private HumedadTotal[] Humedades;
        private Humedad3[] Humedades3;

        private RoutedEventHandler PaginaAnalisis;
        public event RoutedEventHandler VerAnalisis
        {
            add { PaginaAnalisis += value; }
            remove { PaginaAnalisis -= value; }
        }

        public PageAnalisisEquipo()
        {
            InitializeComponent();
        }

        public void SetDataPage(MuestraAnalisis[] muestras, Analisis[] analisis, EnsayoPNT ensayo, Boolean sinHumedades = false)
        {
            MuestrasAnalisis = muestras;
            ListaAnalisis = analisis;
            Ensayo = ensayo;
            SinHumedades = sinHumedades;

            Humedades = FactoriaHumedadTotal.GetHumedades(muestras.Where(m => m.MaterialReferencia == false).Select(m => m.Id).ToArray());
            Humedades3 = FactoriaHumedad3.GetHumedades(muestras.Where(m => m.MaterialReferencia == false).Select(m => m.Id).ToArray());
            GenerarAnalisis();

            if (SinHumedades)
                bNuevo.Visibility = Visibility.Collapsed;
        }

        private void GenerarAnalisis()
        {
            CrearPanelAnalisis();
            CrearGridAnalisis();

            panelAnalisis.InnerValue = new Analisis();
        }

        private void CrearPanelAnalisis()
        {
            FieldSettings fields = new FieldSettings
            {
                ["IdMuestra"] = PropertyControlSettingsEnum.ComboBoxDefault
                                .SetInnerValues(MuestrasAnalisis.Where(m => m.MaterialReferencia == false).ToArray())
                                .SetLabel("Muestra")
                                .AddSelectionChanged((s, e) =>
                                {
                                    if (!SinHumedades)
                                        SeleccionCombo();
                                }),
            };
            if (!SinHumedades)
            {
                fields.Add("IdHumedad", PropertyControlSettingsEnum.ComboBoxDefault
                                    .SetInnerValues(null)
                                    .SetLabel("Humedad"));
                fields.Add("IdHumedad3", PropertyControlSettingsEnum.ComboBoxDefault
                                    .SetInnerValues(null)
                                    .SetLabel("Humedad 3"));
            }

            panelAnalisis.Build(new Analisis(),
                new TypePanelSettings<Analisis>
                {
                    Fields = fields
                });

        }

        private void CrearGridAnalisis()
        {
            ColumnGridSettings columns = new ColumnGridSettings
            {
                ["IdMuestra2"] = new TypeGridColumnSettings
                {
                    Label = "Muestra",
                    ColumnCombo = new TypeGCComboSettings
                    {
                        InnerValues = MuestrasAnalisis,
                        Path = "Id2"
                    }
                },
                ["IdHumedad"] = new TypeGridColumnSettings
                {
                    Label = "Humedad",
                    ColumnCombo = new TypeGCComboSettings
                    {
                        InnerValues = Humedades
                    }
                },
                ["IdHumedad3"] = new TypeGridColumnSettings
                {
                    Label = "Humedad 3",
                    ColumnCombo = new TypeGCComboSettings
                    {
                        InnerValues = Humedades3
                    }
                },
                /*CCI es un boolean, si true genero botón, si false no lo genero*/
                //["CCI"] = new TypeGridColumnSettings
                //{
                //    Label = "Editar CCI",
                //    ColumnButton = new TypeGCButtonSettings
                //    {
                //        DesingPath = CSVPath.EditIcon,
                //        Color = Colors.White,
                //        Size = 25,
                //        Margin = 4,
                //        Click = (sender, e) =>
                //        {
                //            PaginaCCI(sender, e);
                //        }
                //    }
                //},
                ["Editar"] = new TypeGridColumnSettings
                {
                    ColumnButton = new TypeGCButtonSettings
                    {
                        DesingPath = CSVPath.EditIcon,
                        Color = Colors.White,
                        Size = 25,
                        Margin = 4,
                        Click = (sender, e) =>
                        {
                            PaginaAnalisis(sender, e);
                        }
                    }
                },
                ["Borrar"] = new TypeGridColumnSettings
                {
                    Label = "Borrar",
                    ColumnButton = new TypeGCButtonSettings
                    {
                        DesingPath = CSVPath.RemoveIcon,
                        Color = Colors.White,
                        Size = 25,
                        Margin = 4,
                        Click = (sender, e) =>
                        {
                            BorrarAnalisis(sender, e);
                        }
                    }
                }
            };

            if (SinHumedades)
            {
                columns.Remove("IdHumedad");
                columns.Remove("IdHumedad3");
            }

            gridAnalisis.Build<Analisis>(
                new TypeGridSettings
                {
                    Columns = columns,
                    ForegroundRow = (a) =>
                    {
                        Analisis analisis = a as Analisis;
                        if (analisis != null && analisis.CCI)
                            return new SolidColorBrush(Colors.Blue);
                        return null;
                    },
                    CanSortColumns = false,
                    CanResizeColumns = false,
                    SelectionChanged = (s, e) =>
                    {
                        if (!SinHumedades)
                            SeleccionTabla(s, e);
                        else
                            panelAnalisis.InnerValue = new Analisis();
                    }
                });

            gridAnalisis.FillDataGrid(ListaAnalisis);
        }

        public void RecargarPagina()
        {
            ListaAnalisis = RecargarAnalisis();
            gridAnalisis.FillDataGrid(ListaAnalisis);
        }

        private void SeleccionTabla(object s, SelectionChangedEventArgs e)
        {
            var selectedItem = (s as DataGrid).SelectedItem;
            if (selectedItem != null)
            {
                Analisis analisisSeleccionado = selectedItem as Analisis;
                if (analisisSeleccionado.CCI)
                    panelAnalisis.Visibility = Visibility.Collapsed;
                else
                {
                    panelAnalisis.Visibility = Visibility.Visible;
                    panelAnalisis["IdMuestra"].Enabled = false;
                }
            }
        }

        private void SeleccionCombo()
        {
            Analisis analisis = panelAnalisis.InnerValue as Analisis;
            if (analisis != null && !analisis.CCI)
            {
                FillHumedadesMuestra(analisis);
            }
        }

        private void FillHumedadesMuestra(Analisis analisisSeleccionado)
        {
            HumedadTotal[] humedades = FactoriaHumedadTotal.GetHumedades(analisisSeleccionado.IdMuestra).Where(h => (h.MediaHumedadTotalCalculado ?? 0) != 0).ToArray();
            Humedad3[] humedades3 = FactoriaHumedad3.GetHumedades(analisisSeleccionado.IdMuestra).Where(h => (h.MediaHumedadTotalCalculado ?? 0) != 0).ToArray();
            HumedadTotal humedadSeleccionada = humedades.Where(h => h.Id == (analisisSeleccionado.IdHumedad ?? 0)).FirstOrDefault();
            Humedad3 humedad3Seleccionada = humedades3.Where(h => h.Id == (analisisSeleccionado.IdHumedad3 ?? 0)).FirstOrDefault();

            panelAnalisis["IdHumedad"].InnerValues = humedades;
            panelAnalisis["IdHumedad"].SetInnerContent(humedadSeleccionada);
            panelAnalisis["IdHumedad3"].InnerValues = humedades3;
            panelAnalisis["IdHumedad3"].SetInnerContent(humedad3Seleccionada);
        }

        private void ClearHumedadesMuestra()
        {
            panelAnalisis["IdHumedad"].InnerValues = null;
            panelAnalisis["IdHumedad3"].InnerValues = null;
        }

        private void guardar_Click(object sender, RoutedEventArgs e)
        {
            Analisis analisis = panelAnalisis.InnerValue as Analisis;
            if (!analisis?.CCI ?? false)
            {
                if (analisis.Id == 0)
                    GuardarAnalisis(analisis);
                else
                    ActualizarAnalisis(analisis);
            }
        }

        private void GuardarAnalisis(Analisis analisis)
        {
            if (ListaAnalisis.Any(a => a.CCI == analisis.CCI && a.IdMuestra == analisis.IdMuestra))
            {
                MessageBox.Show("Ya añadiste esta muestra al ensayo, por favor edita la existente");
            }
            else
            {
                analisis.Orden = ListaAnalisis.Select(a => a.Orden).DefaultIfEmpty(0).Max() + 1;
                MuestraEnsayo muestra = analisis.ConversorMuestra();
                muestra.IdEnsayo = Ensayo.Id;
                /* insert object */
                int idMuestra = muestra.Insert();
                analisis.Id = idMuestra;
                /*update grid*/
                int size = ListaAnalisis.Count();
                Array.Resize(ref ListaAnalisis, size + 1);
                ListaAnalisis[size] = analisis;
                gridAnalisis.FillDataGrid(ListaAnalisis);

                gridAnalisis.DataGrid.SelectedIndex = gridAnalisis.DataGrid.Items.Count - 1;
            }
        }

        private void ActualizarAnalisis(Analisis analisis)
        {
            MuestraEnsayo muestra = analisis.ConversorMuestra();
            muestra.IdEnsayo = Ensayo.Id;
            /* update object */
            muestra.Update();
            /* update grid */
            int indice = ListaAnalisis.ToList().FindIndex(l => l.CCI == false && l.Id == analisis.Id);
            ListaAnalisis[indice] = analisis;
            gridAnalisis.FillDataGrid(ListaAnalisis);
            gridAnalisis.DataGrid.SelectedIndex = indice;
        }

        private void BorrarAnalisis(object sender, RoutedEventArgs e)
        {
            using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
            using (NpgsqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    //Analisis analisis = panelAnalisis.InnerValue as Analisis;
                    Analisis analisis = gridAnalisis.SelectedItem as Analisis;
                    if (analisis != null)
                    {
                        if (!analisis.CCI)
                        {
                            MuestraEnsayo muestra = analisis.ConversorMuestra();
                            muestra.IdEnsayo = Ensayo.Id;
                            if (FactoriaMuestraEnsayo.ExisteReplica(muestra))
                                MessageBox.Show("No se puede borrar el ensayo, tiene réplicas asociadas");
                            else
                            {
                                muestra.Delete(conn);
                                ActualizarGridBorradoAnalisis(analisis);
                            }
                        }
                        else
                        {
                            MessageBoxResult messageBoxResult = MessageBox.Show("¿Estas seguro que deseas eliminar el CCI? Una vez eliminado, sus datos desaparecerán definitavamente", "Borrar trabajo", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                            if (messageBoxResult == MessageBoxResult.Yes)
                            {
                                BorrarAnalisisCci(analisis);
                                ActualizarGridBorradoAnalisis(analisis);
                            }
                        }
                        trans.Commit();
                    }
                }
                catch (Exception ex)
                {
                    if (trans != null)
                        trans.Rollback();

                    CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "Error al borrar el análisis", ex);
                    MessageBox.Show("Error al borrar el análisis. Por favor, informa a soporte.");
                }
            }
        }

        private void ActualizarGridBorradoAnalisis(Analisis analisis)
        {

            int indice = ListaAnalisis.ToList().FindIndex(l => l.CCI == analisis.CCI && l.Id == analisis.Id);
            List<Analisis> lista = ListaAnalisis.ToList();
            lista.RemoveAt(indice);
            ListaAnalisis = lista.ToArray();
            gridAnalisis.FillDataGrid(ListaAnalisis);
        }
        private void nuevo_Click(object sender, RoutedEventArgs e)
        {
            panelAnalisis.Visibility = Visibility.Visible;
            gridAnalisis.DataGrid.UnselectAll();
            panelAnalisis["IdMuestra"].Enabled = true;
            ClearHumedadesMuestra();

            panelAnalisis.InnerValue = new Analisis();
        }

        private void arriba_Click(object sender, RoutedEventArgs e)
        {
            Desplazar(true);
        }

        private void abajo_Click(object sender, RoutedEventArgs e)
        {
            Desplazar(false);
        }

        private void Desplazar(bool arriba)
        {
            Analisis analisisDesplazado;
            Analisis analisisSeleccionado = gridAnalisis.DataGrid.SelectedItem as Analisis;
            if (analisisSeleccionado != null)
            {
                int indiceDesplazado;
                int indiceSeleccionado = ListaAnalisis.ToList().FindIndex(l => l.CCI == analisisSeleccionado.CCI && l.Id == analisisSeleccionado.Id);
                int numeroAnalisis = ListaAnalisis.Count();

                if ((arriba && indiceSeleccionado > 0) || (!arriba && indiceSeleccionado < numeroAnalisis - 1))
                {
                    indiceDesplazado = (arriba) ? indiceSeleccionado - 1 : indiceSeleccionado + 1;
                    analisisDesplazado = ListaAnalisis[indiceDesplazado];

                    /* cambio ordenes */
                    int ordenSeleccionado = analisisSeleccionado.Orden;
                    analisisSeleccionado.Orden = analisisDesplazado.Orden;
                    analisisDesplazado.Orden = ordenSeleccionado;

                    /*cambio orden en la lista*/
                    ListaAnalisis[indiceDesplazado] = analisisSeleccionado;
                    ListaAnalisis[indiceSeleccionado] = analisisDesplazado;
                    gridAnalisis.FillDataGrid(ListaAnalisis);

                    /* guardo en BBDD y actualizo Grid*/
                    gridAnalisis.DataGrid.SelectedIndex = indiceDesplazado;
                    actualizarOrden(analisisDesplazado);
                    actualizarOrden(analisisSeleccionado);
                }
            }
        }

        private void actualizarOrden(Analisis analisis)
        {
            if (analisis.CCI)
            {
                GuardarOrden(analisis);
            }
            else
            {
                MuestraEnsayo muestra = analisis.ConversorMuestra();
                muestra.Update(null, "OrdenEnsayo");
            }
        }

        private void nuevoCCI_Click(object sender, RoutedEventArgs e)
        {
            gridAnalisis.DataGrid.UnselectAll();
            PaginaAnalisis(sender, e);
        }
    }

    public class Analisis
    {
        public int Id { get; set; }

        public int IdMuestra { get; set; }

        public int Orden { get; set; }

        public int? IdTecnico { get; set; }

        public int? IdHumedad { get; set; }

        public int? IdHumedad3 { get; set; }

        public bool CCI { get; set; }

        public String IdMuestra2
        {
            get { return (CCI == true ? "CCI" : "") + IdMuestra; }
            set { }
        }

        public MuestraEnsayo ConversorMuestra()
        {
            return new MuestraEnsayo
            {
                Id = this.Id,
                IdHumedad = this.IdHumedad,
                IdHumedad3 = this.IdHumedad3,
                OrdenEnsayo = this.Orden,
                IdMuestra = this.IdMuestra,
            };
        }

        public ChnControl ConversorControlChn()
        {
            return new ChnControl
            {
                Id = this.Id,
                OrdenEnsayo = this.Orden
            };
        }

        public FusibilidadControl ConversorControlFus()
        {
            return new FusibilidadControl
            {
                Id = this.Id,
                OrdenEnsayo = this.Orden
            };
        }
    }

    public class MuestraAnalisis
    {
        public int Id { get; set; }

        public String Nombre { get; set; }

        public bool MaterialReferencia { get; set; }

        public String Id2
        {
            get { return (MaterialReferencia == true ? "CCI" : "") + Id; }
            set { }
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
