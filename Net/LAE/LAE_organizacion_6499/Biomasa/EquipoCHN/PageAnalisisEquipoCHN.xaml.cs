using Cartif.Logs;
using Cartif.XamlResources;
using GenericForms.Abstract;
using GenericForms.Settings;
using LAE.Biomasa.Modelo;
using Npgsql;
using LAE.Comun.Persistence;
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
using LAE.Comun.Modelo.Procedimientos;
using LAE.Biomasa.Modelo;

namespace LAE.Biomasa.Pages
{
    /// <summary>
    /// Lógica de interacción para PageAnalisisEquipoCHN.xaml
    /// </summary>
    public partial class PageAnalisisEquipoCHN : UserControl
    {
        private EnsayoPNT ensayo;
        public EnsayoPNT Ensayo
        {
            get { return ensayo; }
            set
            {
                ensayo = value;
                FillListaAnalisis();
                CrearPanelAnalisis();
            }
        }

        public AnalisisCHN[] ListaAnalisis;
        private MuestraCHN[] MuestrasCHN;

        private HumedadTotal[] Humedades;
        private Humedad3[] Humedades3;

        private RoutedEventHandler PaginaCCI;
        public event RoutedEventHandler VerCCI
        {
            add { PaginaCCI += value; }
            remove { PaginaCCI -= value; }
        }

        public PageAnalisisEquipoCHN()
        {
            InitializeComponent();

            Humedades = PersistenceManager.SelectAll<HumedadTotal>().ToArray();
            Humedades3 = PersistenceManager.SelectAll<Humedad3>().ToArray();

            FillListaMuestras();
        }

        private void FillListaMuestras()
        {
            MuestraRecepcionBiomasa[] muestras = FactoriaMuestraRecepcionBiomasa.GetMuestrasByProcedimiento("CHN");
            ChnMaterialReferencia[] materiales = PersistenceManager.SelectAll<ChnMaterialReferencia>().ToArray();

            List<MuestraCHN> muestrasCHN = muestras.Select(m => new MuestraCHN()
            {
                Id = m.Id,
                MaterialReferencia = false,
                Nombre = m.GetCodigoLae
            }).ToList();

            muestrasCHN.AddRange(materiales.Select(m => new MuestraCHN()
            {
                Id = m.Id,
                MaterialReferencia = true,
                Nombre = m.ToString()
            }));

            MuestrasCHN = muestrasCHN.OrderBy(m => m.Id).ToArray();
        }

        private void FillListaAnalisis()
        {
            ListaAnalisis = GetAnalisis();
        }

        private AnalisisCHN[] GetAnalisis()
        {
            MuestraEnsayo[] muestrasEnsayo = PersistenceManager.SelectByProperty<MuestraEnsayo>("IdEnsayo", Ensayo.Id).ToArray();
            ChnControl[] controlesCalidad = PersistenceManager.SelectByProperty<ChnControl>("IdEnsayo", Ensayo.Id).ToArray();

            List<AnalisisCHN> lista = muestrasEnsayo.Select(l => new AnalisisCHN()
            {
                Id = l.Id,
                IdMuestra = l.IdMuestra,
                Orden = l.OrdenEnsayo,
                IdHumedad = l.IdHumedad,
                IdHumedad3 = l.IdHumedad3,
                CCI = false
            }).ToList();

            lista.AddRange(controlesCalidad.Select(c => new AnalisisCHN()
            {
                Id = c.Id,
                IdMuestra = c.IdMaterialReferencia,
                Orden = c.OrdenEnsayo,
                IdTecnico = c.IdTecnico,
                CCI = true
            }));
            return lista.OrderBy(l => l.Orden).ToArray();
        }

        private void CrearPanelAnalisis()
        {
            panelAnalisis.Build(new AnalisisCHN(),
                new TypePanelSettings<AnalisisCHN>
                {
                    Fields = new FieldSettings
                    {
                        ["IdMuestra"] = PropertyControlSettingsEnum.ComboBoxDefault
                                    .SetInnerValues(MuestrasCHN.Where(m => m.MaterialReferencia == false).ToArray())
                                    .SetLabel("Muestra")
                                    .AddSelectionChanged((s, e) => SeleccionCombo()),
                        ["IdHumedad"] = PropertyControlSettingsEnum.ComboBoxDefault
                                    .SetInnerValues(null)
                                    .SetLabel("Humedad"),
                        ["IdHumedad3"] = PropertyControlSettingsEnum.ComboBoxDefault
                                    .SetInnerValues(null)
                                    .SetLabel("Humedad 3"),
                    },
                });

            gridAnalisis.Build(ListaAnalisis, new TypeGridSettings
            {
                Columns = new ColumnGridSettings
                {
                    ["Id"] = TypeGridColumnSettingsEnum.DefaultColum,
                    ["Orden"] = TypeGridColumnSettingsEnum.DefaultColum,
                    ["IdMuestra2"] = new TypeGridColumnSettings
                    {
                        Label = "Muestra",
                        ColumnCombo = new TypeGCComboSettings
                        {
                            InnerValues = MuestrasCHN,
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
                    ["CCI"] = new TypeGridColumnSettings
                    {
                        Label = "Editar CCI",
                        ColumnButton = new TypeGCButtonSettings
                        {
                            DesingPath = CSVPath.EditIcon,
                            Color = Colors.White,
                            Size = 25,
                            Margin = 4,
                            Click = (sender, e) =>
                            {
                                PaginaCCI(sender, e);
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
                                borrar(sender, e);
                            }
                        }
                    }
                },
                ForegroundRow = (a) =>
                {
                    AnalisisCHN analisis = a as AnalisisCHN;
                    if (analisis != null && analisis.CCI)
                        return new SolidColorBrush(Colors.Blue);
                    return null;
                },
                CanSortColumns = false,
                CanResizeColumns = false,
                SelectionChanged = (s, e) => SeleccionTabla(s, e)
            });


            panelAnalisis.InnerValue = new AnalisisCHN();
        }

        public void RecargarPagina()
        {
            ListaAnalisis = GetAnalisis();
            gridAnalisis.FillDataGrid(ListaAnalisis);
        }

        private void SeleccionTabla(object s, SelectionChangedEventArgs e)
        {
            var selectedItem = (s as DataGrid).SelectedItem;
            if (selectedItem != null)
            {
                AnalisisCHN analisisSeleccionado = selectedItem as AnalisisCHN;
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
            AnalisisCHN analisis = panelAnalisis.InnerValue as AnalisisCHN;
            if (analisis != null && !analisis.CCI)
            {
                FillHumedadesMuestra(analisis);
            }
        }

        private void FillHumedadesMuestra(AnalisisCHN analisisSeleccionado)
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
            AnalisisCHN analisis = panelAnalisis.InnerValue as AnalisisCHN;
            if (!analisis?.CCI ?? false)
            {
                if (analisis.Id == 0)
                    GuardarAnalisis(analisis);
                else
                    ActualizarAnalisis(analisis);
            }
        }

        private void GuardarAnalisis(AnalisisCHN analisis)
        {
            if (ListaAnalisis.Any(a => a.CCI==analisis.CCI && a.IdMuestra == analisis.IdMuestra))
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

        private void ActualizarAnalisis(AnalisisCHN analisis)
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

        private void borrar(object sender, RoutedEventArgs e)
        {
            using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
            using (NpgsqlTransaction trans = conn.BeginTransaction())
            {
                try
                {

                    AnalisisCHN analisis = panelAnalisis.InnerValue as AnalisisCHN;
                    if (analisis != null)
                    {
                        if (!analisis.CCI)
                        {
                            MuestraEnsayo muestra = analisis.ConversorMuestra();
                            muestra.Delete(conn);
                        }
                        else
                        {
                            ChnControl control = analisis.ConversorControl();
                            control.Replicas = PersistenceManager.SelectByProperty<ReplicaChnControl>("IdCHN", control.Id).ToList();
                            PersistenceDataManipulation.Borrar(conn, control.Replicas);
                            control.Delete(conn);
                        }
                        ActualizarGridBorradoAnalisis(analisis);
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

        private void ActualizarGridBorradoAnalisis(AnalisisCHN analisis)
        {

            int indice = ListaAnalisis.ToList().FindIndex(l => l.CCI == analisis.CCI && l.Id == analisis.Id);
            List<AnalisisCHN> lista = ListaAnalisis.ToList();
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

            panelAnalisis.InnerValue = new AnalisisCHN();
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
            AnalisisCHN analisisDesplazado;
            AnalisisCHN analisisSeleccionado = gridAnalisis.DataGrid.SelectedItem as AnalisisCHN;
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

        private void actualizarOrden(AnalisisCHN analisis)
        {
            if (analisis.CCI)
            {
                ChnControl control = analisis.ConversorControl();
                control.Update(null, "OrdenEnsayo");
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
            PaginaCCI(sender, e);
        }
    }

    public class AnalisisCHN
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
                IdMuestra = this.IdMuestra
            };
        }

        public ChnControl ConversorControl()
        {
            return new ChnControl
            {
                Id = this.Id,
                OrdenEnsayo = this.Orden
            };
        }
    }

    public class MuestraCHN
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
