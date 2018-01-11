using Cartif.Extensions;
using Cartif.Logs;
using Cartif.Util;
using Cartif.XamlResources;
using Dapper;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using GUI.Controls;
using LAE.Modelo;
using MahApps.Metro.Controls;
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
using System.Windows.Shapes;
using System.Collections.Specialized;
using Cartif.Expectation;
using LAE.Clases;
using LAE.Comun.Modelo;
using LAE.Comun.Clases;

namespace GUI.Windows
{
    /// <summary>
    /// Lógica de interacción para TomasMuestra.xaml
    /// </summary>
    public partial class TomasMuestraAgua : MetroWindow
    {
        public static readonly DependencyProperty IconTitleProperty =
               DependencyProperty.Register("IconTitle", typeof(ImageSource), typeof(TomasMuestraAgua), new PropertyMetadata(null));
        public ImageSource IconTitle
        {
            get { return (ImageSource)GetValue(IconTitleProperty); }
            set { SetValue(IconTitleProperty, value); }
        }

        private Oferta oferta;
        private Trabajo trabajo;

        private TomaMuestraAgua tomaMuestra;
        public TomaMuestraAgua TomaMuestra
        {
            get { return tomaMuestra; }
            set
            {
                tomaMuestra = value;
                CargarDatos();
            }
        }

        public TomasMuestraAgua(Oferta o, Trabajo t, TomaMuestraAgua toma)
        {
            InitializeComponent();
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE.Comun;component/images/cabecera.png", UriKind.Absolute));
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            oferta = o;
            trabajo = t;
            TomaMuestra = toma;
            GenerarPanelDatos();

        }

        private void GenerarPanelDatos()
        {
            GenerarCliente();
            GenerarContacto();
        }

        private void CargarDatos()
        {
            TomaMuestra.BlancosMuestreo = new List<BlancomuestreoTMAgua>() { new BlancomuestreoTMAgua() };


            if (TomaMuestra?.Id != 0)
                CargarMuestras();
            else
                GenerarMuestras();

            GenerarBlancoMuestreo();
            TomaMuestra.LoadData();
        }

        private void GenerarCliente()
        {
            Cliente c = PersistenceManager.SelectByID<Cliente>(oferta.IdCliente);

            panelCliente.Build(c,
                new TypePanelSettings<Cliente>
                {
                    Fields = new FieldSettings
                    {
                        ["Nombre"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetEnabled(false)
                            .SetLabel("Cliente"),
                        ["Telefono"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetEnabled(false),
                        ["Email"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetEnabled(false),
                    },
                    IsUpdating = true
                });
        }

        private void GenerarContacto()
        {
            Contacto c = PersistenceManager.SelectByID<Contacto>(trabajo.IdContacto);
            panelContacto.Build(c,
                new TypePanelSettings<Contacto>
                {
                    ColumnWidths = new int[] { 1, 1, 1, 1 },
                    Fields = new FieldSettings
                    {
                        ["Nombre"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["Apellidos"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["Telefono"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["Email"] = PropertyControlSettingsEnum.TextBoxDefault,
                    },
                    DefaultSettings = new PropertyControlSettings
                    {
                        Enabled = false
                    }
                });
        }

        private void CargarMuestras()
        {
            MuestraAgua[] muestras = PersistenceManager.SelectByProperty<MuestraAgua>("IdTomaMuestra", TomaMuestra.Id).ToArray();
            muestras.ForEach(m =>
            {
                ControlMuestraAgua cma = AddMuestra(m);
                m.LoadData();
                cma.panelTipoMuestra.InnerValue = cma.Muestra.TipoMuestra;
            });
        }

        private void GenerarMuestras()
        {
            PuntocontrolRevision[] puntosControl = FactoriaPuntocontrolRevision.GetPuntosControlConTomaMuestra(TomaMuestra.IdTrabajo);
            int i = 0;
            puntosControl.ForEach(pc =>
            {
                i++;

                pc.LoadLineasRevision();
                MuestraAgua ma = AddMuestra().Muestra;
                ma.NumCodigo = i;

                pc.LineasRevision.ForEach(l =>
                {
                    ma.IdPuntoControl = pc.Id;

                    Parametro p = new Parametro { Id = l.IdParametro };
                    if (FactoriaParametros.GetParametrosByTipo("Aguas").Contains(p) && p.Id != FactoriaParametros.IDTOMAMUESTRA)
                        ma.ParametrosLaboratorio.Add(new ParamsLaboratorioMuestraAgua { IdParametro = l.IdParametro });
                });
            });
        }

        private void GenerarBlancoMuestreo()
        {

            int[] idParams = listaMuestras.Children.OfType<ControlMuestraAgua>()
                .Where(c => c.IsEnabled)
                .Select(c => c.Muestra)
                .SelectMany(m => m.ParametrosLaboratorio)
                .Select(p => p.IdParametro).Distinct().ToArray();

            Parametro[] listaParam = new Parametro[idParams.Count()];
            for (int i = 0; i < idParams.Count(); i++)
            {
                listaParam[i] = (PersistenceManager.SelectByID<Parametro>(idParams[i]));
            }

            panelBlancoMuestreo.Build(new BlancomuestreoTMAgua(),
                new TypePanelSettings<BlancomuestreoTMAgua>
                {
                    Fields = new FieldSettings
                    {
                        ["IdParametro"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(listaParam)
                            .SetLabel("Parámetro")
                            .AddKeyDownCombo(
                                (sender, e) => { if (e.Key == Key.Enter) AddBlancoMuestro(); }
                            ),
                        ["Conforme"] = new PropertyControlSettings
                        {
                            InnerValues = new ComboBoxItem<Boolean>[]
                            {
                                ComboBoxItem<Boolean>.Create("Si",true),
                                ComboBoxItem<Boolean>.Create("No",false)
                            },
                            Type = typeof(PropertyControlComboBox)
                        },
                        ["BotonAdd"] = new PropertyControlSettings
                        {
                            Type = typeof(PropertyControlButton),
                            DesignPath = CSVPath.ArrowRightIcon,
                            Click = (sender, e) => { AddBlancoMuestro(); }
                        }
                    }
                });

            gridBlancoMuestreo.Build<BlancomuestreoTMAgua>(
                new TypeGridSettings
                {
                    Columns = new ColumnGridSettings
                    {
                        ["IdParametro"] = new TypeGridColumnSettings
                        {
                            Label = "Parámetro",
                            Width = 3,
                            ColumnCombo = new TypeGCComboSettings
                            {
                                InnerValues = FactoriaParametros.GetParametrosByTipo("Aguas"),
                                Path = "Id"
                            }
                        },
                        ["Conforme"] = new TypeGridColumnSettings
                        {
                            ColumnCombo = new TypeGCComboSettings
                            {
                                InnerValues = new ComboBoxItem<Boolean>[]
                                {
                                    ComboBoxItem<Boolean>.Create("Si",true),
                                    ComboBoxItem<Boolean>.Create("No",false)
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
                                    DeleteBlancoMuestreo(sender);
                                }
                            }
                        }
                    }
                });

            gridBlancoMuestreo.FillDataGrid(TomaMuestra.BlancosMuestreo);


            panelCodigoBlanco.Build(TomaMuestra,
                new TypePanelSettings<TomaMuestraAgua>
                {
                    ColumnWidths = new int[] { 1 },
                    Fields = new FieldSettings
                    {
                        ["GetCodigoBlanco"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetLabel("")
                            .SetEnabled(true)
                    }
                });
        }

        private void AddBlancoMuestro()
        {
            if (!panelBlancoMuestreo.AddElementToList(TomaMuestra.BlancosMuestreo))
                MessageBox.Show("Datos erróneos. Por favor, revisa la información");
        }

        private void DeleteBlancoMuestreo(object sender)
        {
            if (TomaMuestra.BlancosMuestreo.Count > 1 || TomaMuestra.NumBlanco == null || TomaMuestra.LastNumBlanco())
            {
                BlancomuestreoTMAgua lineaBorrada = ((FrameworkElement)sender).DataContext as BlancomuestreoTMAgua;
                TomaMuestra.BlancosMuestreo.Remove(lineaBorrada);
            }
            else {
                MessageBox.Show("No se puede borrar el último parámetro del Blanco de Muestreo, porque existen códigos de Blanco de Muestreo posteriores al código asignado en esta Toma de Muestra");
            }
        }

        //private void NuevaMuestra_Click(object sender, RoutedEventArgs e)
        //{
        //    AddMuestra();
        //}

        private ControlMuestraAgua AddMuestra()
        {
            MuestraAgua muestraAgua = new MuestraAgua() { IdTecnico = trabajo.IdTecnico };
            return AddMuestra(muestraAgua);
        }

        private ControlMuestraAgua AddMuestra(MuestraAgua muestraAgua)
        {
            muestraAgua.Materiales = new List<MaterialesMuestraAgua>();
            muestraAgua.Localizaciones = new List<LocalizacionesMuestraAgua>() ;
            muestraAgua.TipoMuestra = new TiposMuestraMuestraAgua();
            muestraAgua.ParametrosInsitu = new List<ParamsInsituMuestraAgua>();
            muestraAgua.ParametrosLaboratorio = new List<ParamsLaboratorioMuestraAgua>();
            muestraAgua.Alicuotas = new List<AlicuotaMuestraAgua>();
            muestraAgua.Equipos = new List<EquiposMuestraAgua>();

            muestraAgua.GetCodigoLae = String.Format("{0}-SE-{1:0#}-TM-{2}", oferta.Codigo, trabajo.NumCodigo, (TomaMuestra.NumCodigo != 0 ? String.Format("{0:0#}", TomaMuestra.NumCodigo) : "XX"));
            //muestraAgua.GetCodigoLae = oferta.Codigo + "-TM-"+ (TomaMuestra.NumCodigo!=0? String.Format("{0:0#}", TomaMuestra.NumCodigo):"XX");

            ControlMuestraAgua cma = new ControlMuestraAgua() { Muestra = muestraAgua };
            listaMuestras.Children.Add(cma);
            cma.DisabledControl = (lc) => DisabledMuestraAgua(lc);
            cma.EnabledControl = (lc) => EnabledMuestraAgua(lc);

            return cma;
        }

        private void DisabledMuestraAgua(ControlMuestraAgua lc)
        {
            lc.SetEnabledControls(false);
        }

        private void EnabledMuestraAgua(ControlMuestraAgua lc)
        {
            lc.SetEnabledControls(true);
        }

        private void bGuardar_Click(object sender, RoutedEventArgs e)
        {

            if (validarTomaMuestra())
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                using (NpgsqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (TomaMuestra.Id == 0)
                        {
                            String consulta = "select insertartomaagua(@IdTrabajo)";
                            int idTomaMuestra = conn.Query<int>(consulta, TomaMuestra).FirstOrDefault();
                            TomaMuestra.Id = idTomaMuestra;
                            //int idTomaMuestra = PersistenceDataManipulation.Guardar(conn, TomaMuestra);
                        }
                        else
                        {
                            TomaMuestra.Update(conn);
                        }
                        GuardarDatos(conn, TomaMuestra.Id);
                        BorrarDatos(conn, TomaMuestra.Id);

                        trans.Commit();
                        MessageBox.Show("Datos guardados con éxito");
                    }
                    catch (Exception ex)
                    {
                        if (trans != null)
                            trans.Rollback();

                        CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "Error al guardar la toma de muestra", ex);
                        MessageBox.Show("Se ha producido un error al intentar guardar los datos. Por favor, recargue la página o informa a soporte.");
                        this.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Datos erróneos. Por favor, revisa la información");
            }
        }

        private bool validarTomaMuestra()
        {
            var muestras = listaMuestras.Children.OfType<ControlMuestraAgua>();

            foreach (ControlMuestraAgua item in muestras)
            {
                if (!item.Validar())
                    return false;
            }
            return true;
        }

        private void GuardarDatos(NpgsqlConnection conn, int idTomaMuestra)
        {

            var muestras = listaMuestras.Children.OfType<ControlMuestraAgua>()
                .Where(c => c.IsEnabled)
                .Select(c => c.Muestra);

            /* muestras agua */
            PersistenceDataManipulation.GuardarElement1N(conn, TomaMuestra, muestras, tm => tm.Id, "IdTomaMuestra");

            /* blancos de muestreo */
            PersistenceDataManipulation.GuardarElement1N(conn, TomaMuestra, TomaMuestra.BlancosMuestreo.ToList(), tm => tm.Id, "IdTomaMuestra");
            if (TomaMuestra.BlancosMuestreo.Count > 0)
            {
                String consulta = "select asignarblancotomaagua(@Id)";
                int? numBlanco = conn.Query<int?>(consulta, TomaMuestra).FirstOrDefault();
                if (numBlanco == null)
                    throw new PersistenceDataException("Error al guardar blanco de muestreo");
            }
            else
            {
                TomaMuestra.NumBlanco = null;
                TomaMuestra.Update(conn);
            }

            /* lista 1 elemento */
            PersistenceDataManipulation.GuardarListadoN1(conn, muestras, ma => ma.TipoMuestra, ma => ma.Id, "IdMuestra");

            /* listas de cada muestra (materiales, localizaciones, ...) */
            PersistenceDataManipulation.GuardarListadoNN(conn, muestras, ma => ma.Materiales, ma => ma.Id, "IdMuestra");
            PersistenceDataManipulation.GuardarListadoNN(conn, muestras, ma => ma.Localizaciones, ma => ma.Id, "IdMuestra");
            PersistenceDataManipulation.GuardarListadoNN(conn, muestras, ma => ma.ParametrosInsitu, ma => ma.Id, "IdMuestra");
            PersistenceDataManipulation.GuardarListadoNN(conn, muestras, ma => ma.ParametrosLaboratorio, ma => ma.Id, "IdMuestra");
            PersistenceDataManipulation.GuardarListadoNN(conn, muestras, ma => ma.Alicuotas, ma => ma.Id, "IdMuestra");
            PersistenceDataManipulation.GuardarListadoNN(conn, muestras, ma => ma.Equipos, ma => ma.Id, "IdMuestra");
        }

        private void BorrarDatos(NpgsqlConnection conn, int idTomaMuestra)
        {
            var muestras = listaMuestras.Children.OfType<ControlMuestraAgua>()
                .Where(c => c.Muestra.Id > 0)
                .Select(c => new Tuple<MuestraAgua, Boolean>(c.Muestra, !c.IsEnabled));

            /* listas de cada muestra (materiales, localizaciones, ...) */
            PersistenceDataManipulation.BorrarListadoNN(conn, muestras, ma => ma.Materiales, ma => ma.Id, "IdMuestra");
            PersistenceDataManipulation.BorrarListadoNN(conn, muestras, ma => ma.Localizaciones, ma => ma.Id, "IdMuestra");
            PersistenceDataManipulation.BorrarListadoNN(conn, muestras, ma => ma.ParametrosInsitu, ma => ma.Id, "IdMuestra");
            PersistenceDataManipulation.BorrarListadoNN(conn, muestras, ma => ma.ParametrosLaboratorio, ma => ma.Id, "IdMuestra");
            PersistenceDataManipulation.BorrarListadoNN(conn, muestras, ma => ma.Alicuotas, ma => ma.Id, "IdMuestra");
            PersistenceDataManipulation.BorrarListadoNN(conn, muestras, ma => ma.Equipos, ma => ma.Id, "IdMuestra");


            PersistenceDataManipulation.Borrar1N<BlancomuestreoTMAgua>(conn, TomaMuestra.BlancosMuestreo.ToList(), TomaMuestra.Id, "IdTomaMuestra");

            /* con la modificación ya no se pueden borrar muestrass*/
            /*
            List<MuestraAgua> muestrasBorrar = muestras.Where(m => m.Item2 == true).Select(m => m.Item1).ToList();
            List<TiposMuestraMuestraAgua> tiposBorrar = muestrasBorrar.Select(m => m.TipoMuestra).ToList();

            // lista 1 elemento
            PersistenceDataManipulation.Borrar(conn, tiposBorrar);

            // muestras agua
            PersistenceDataManipulation.Borrar(conn, muestrasBorrar);
            */
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            this.MensajeCancel();
        }
    }
}
