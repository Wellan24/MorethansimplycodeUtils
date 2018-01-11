using Cartif.Extensions;
using Cartif.Logs;
using Dapper;
using GenericForms.Abstract;
using GenericForms.Settings;
using GUI.Controls;
using GUI.TreeListView.Tabs;
using LAE.Clases;
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
using LAE.Comun.Modelo;
using LAE.Comun.Clases;

namespace GUI.Windows
{
    /// <summary>
    /// Lógica de interacción para RecepcionesMuestra.xaml
    /// </summary>
    public partial class RecepcionesMuestraAgua : MetroWindow
    {
        public static readonly DependencyProperty IconTitleProperty =
            DependencyProperty.Register("IconTitle", typeof(ImageSource), typeof(RecepcionesMuestraAgua), new PropertyMetadata(null));
        public ImageSource IconTitle
        {
            get { return (ImageSource)GetValue(IconTitleProperty); }
            set { SetValue(IconTitleProperty, value); }
        }

        private RecepcionAgua recepcionAgua;
        public RecepcionAgua RecepcionAgua
        {
            get { return recepcionAgua; }
            set
            {
                recepcionAgua = value;
                CargarDatos();
            }
        }

        private int idCliente;
        public int IdCliente
        {
            get { return idCliente; }
            set
            {
                idCliente = value;
                GenerarCliente();
            }
        }

        private int idContacto;
        public int IdContacto
        {
            get { return idContacto; }
            set
            {
                idContacto = value;
                GenerarContacto();
            }
        }

        public RecepcionesMuestraAgua(int idCliente, int idContacto, RecepcionAgua recepcion)
        {
            InitializeComponent();
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE.Comun;component/images/cabecera.png", UriKind.Absolute));
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

            RecepcionAgua = recepcion;
            IdCliente = idCliente;
            IdContacto = idContacto;
        }

        private void CargarDatos()
        {
            GenerarDatosMuestras();
            if (RecepcionAgua?.Id != 0)
            {
                CargarMuestra();
            }
            else
            {
                GenerarMuestra();
            }
        }

        private void GenerarCliente()
        {
            Cliente c = PersistenceManager.SelectByID<Cliente>(IdCliente);

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
            Contacto c = PersistenceManager.SelectByID<Contacto>(IdContacto);
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

        private void GenerarDatosMuestras()
        {
            panelDatos.Build(RecepcionAgua,
                new TypePanelSettings<RecepcionAgua>
                {
                    Fields = new FieldSettings
                    {
                        ["FechaRecepcion"] = PropertyControlSettingsEnum.DateTimeDefaultNoEmpty
                                        .SetLabel("Fecha recepción*"),
                        ["FechaCaducidad"] = PropertyControlSettingsEnum.DateTimeDefault
                                        .SetLabel("Fecha caducidad"),
                        ["IdTecnico"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                        .SetInnerValues(FactoriaTecnicos.GetTecnicos())
                                        .SetLabel("Técnico"),
                    },
                    IsUpdating = true
                });

            panelObservaciones.Build(RecepcionAgua,
                new TypePanelSettings<RecepcionAgua>
                {
                    ColumnWidths = new int[] { 1 },
                    Fields = new FieldSettings
                    {
                        ["Observaciones"] = PropertyControlSettingsEnum.TextBoxDefault
                                .SetHeightMultiline(60)
                    }
                });
        }

        private void CargarMuestra()
        {
            MuestraRecepcionAgua[] muestras = PersistenceManager.SelectByProperty<MuestraRecepcionAgua>("IdRecepcion", RecepcionAgua.Id).ToArray();
            muestras.ForEach(m =>
            {
                ControlMuestraRecepAgua cmra = AddMuestra(m);
                m.LoadData();
                cmra.panelMuestra.InnerValue = cmra.Muestra;

                cmra.treeAlicuotas.Build(cmra.Muestra);
            });
        }

        private void GenerarMuestra()
        {
            PuntocontrolRevision[] puntosControl = FactoriaPuntocontrolRevision.GetPuntosControl(RecepcionAgua.IdTrabajo);

            Parametro[] parametrosAgua = FactoriaParametros.GetParametrosByTipo("Aguas").ToArray();

            TomaMuestraAgua tmAgua = PersistenceManager.SelectByProperty<TomaMuestraAgua>("IdTrabajo", RecepcionAgua.IdTrabajo).FirstOrDefault();

            puntosControl.ForEach(pc =>
            {

                MuestraAgua muestraAgua;
                if (tmAgua != null)
                    muestraAgua = PersistenceManager.SelectByProperty<MuestraAgua>("IdTomaMuestra", tmAgua.Id).Where(m => m.IdPuntoControl == pc.Id).FirstOrDefault();
                else
                    muestraAgua = null;

                /* add muestra */
                ControlMuestraRecepAgua cmra = AddMuestra(muestraAgua);
                MuestraRecepcionAgua ma = cmra.Muestra;

                ma.IdPuntoControl = pc.Id;

                /* load params */
                pc.LoadLineasRevision();
                pc.LineasRevision.ForEach(l =>
                {
                    
                    Parametro p = new Parametro { Id = l.IdParametro };
                    if (parametrosAgua.Contains(p) && p.Id!=FactoriaParametros.IDTOMAMUESTRA)
                        ma.Parametros.Add(new LineaAliRecepcionAgua { IdParametro = l.IdParametro });
                });


                if (muestraAgua != default(MuestraAgua))
                {
                    muestraAgua.LoadAlicuotas();
                    muestraAgua.Alicuotas.ForEach(a => ma.Alicuotas.Add(new AlicuotaRecepcionAgua { NumeroAlicuotas = a.NumAlicuotas, RecipienteVidrio = a.RecipienteVidrio }));
                }

                cmra.treeAlicuotas.Build(ma);

            });


            /*
            TomaMuestraAgua toma = PersistenceManager.SelectByProperty<TomaMuestraAgua>("IdTrabajo", RecepcionAgua.IdTrabajo).FirstOrDefault();

            if (toma != null)
            {
                MuestraAgua[] muestras = PersistenceManager.SelectByProperty<MuestraAgua>("IdTomaMuestra", toma.Id).ToArray();
                muestras.ForEach(m =>
                {
                    m.LoadAlicuotas();
                    m.LoadParametros();
                    ControlMuestraRecepAgua cmra = AddMuestra();

                    m.Alicuotas.ForEach(a =>
                    {
                        cmra.Muestra.Alicuotas.Add(new AlicuotaRecepcionAgua { NumeroAlicuotas = a.NumAlicuotas, RecipienteVidrio = a.RecipienteVidrio });
                    });

                    m.ParametrosLaboratorio.ForEach(p =>
                    {
                        cmra.Muestra.Parametros.Add(new LineaAliRecepcionAgua { IdParametro=p.IdParametro });
                    });
                });
            }
            */
        }

        //private ControlMuestraRecepAgua AddMuestra(bool tieneTM)
        //{
        //    MuestraRecepcionAgua muestra = new MuestraRecepcionAgua() { TieneTomaMuestra = tieneTM };
        //    return AddMuestra(muestra);
        //}

        private ControlMuestraRecepAgua AddMuestra(MuestraAgua muestraAgua)
        {
            MuestraRecepcionAgua muestra;
            if (muestraAgua != default(MuestraAgua))
                muestra = new MuestraRecepcionAgua() { TieneTomaMuestra = true, CodigoToma = muestraAgua.GetCodigoLae };
            else
                muestra = new MuestraRecepcionAgua() { TieneTomaMuestra = false };
            return AddMuestra(muestra);
        }

        private ControlMuestraRecepAgua AddMuestra(MuestraRecepcionAgua muestra)
        {
            muestra.Alicuotas = new ObservableCollection<AlicuotaRecepcionAgua>() { new AlicuotaRecepcionAgua() };
            muestra.Parametros = new ObservableCollection<LineaAliRecepcionAgua>() { new LineaAliRecepcionAgua() };


            ControlMuestraRecepAgua cmra = new ControlMuestraRecepAgua() { Muestra = muestra };

            listaMuestras.Children.Add(cmra);
            cmra.DisabledControl = (lc) => DisabledMuestraRecepAgua(lc);
            cmra.EnabledControl = (lc) => EnabledMuestraRecepAgua(lc);
            return cmra;
        }

        private void DisabledMuestraRecepAgua(ControlMuestraRecepAgua lc)
        {
            lc.SetEnabledControl(false);
        }

        private void EnabledMuestraRecepAgua(ControlMuestraRecepAgua lc)
        {
            lc.SetEnabledControl(true);
        }

        //private void NuevaRecepcion_Click(object sender, RoutedEventArgs e)
        //{
        //    AddMuestra();
        //}

        private void bGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarRecepcion())
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                using (NpgsqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (RecepcionAgua.Id == 0)
                        {
                            //String consulta = "select insertarrecepcionagua(@FechaRecepcion, @FechaCaducidad, @IdTecnico, @IdTrabajo)";
                            //int idRecepcion = conn.Query<int>(consulta, RecepcionAgua).FirstOrDefault();

                            int idRecepcion = PersistenceDataManipulation.Guardar(conn, RecepcionAgua);
                            RecepcionAgua.Id = idRecepcion;
                        }
                        else
                        {
                            RecepcionAgua.Update(conn);
                        }

                        GuardarDatos(conn, RecepcionAgua.Id);
                        BorrarDatos(conn, RecepcionAgua.Id);

                        trans.Commit();
                        MessageBox.Show("Datos guardados con éxito");
                    }
                    catch (Exception ex)
                    {
                        if (trans != null)
                            trans.Rollback();

                        CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "Error al guardar la recepción e inspección de muestra", ex);
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

        private void FillDatosAlicuotas()
        {
            var controles = listaMuestras.Children.OfType<ControlMuestraRecepAgua>()
                                .Where(c => c.IsEnabled);
            foreach (ControlMuestraRecepAgua control in controles)
            {
                control.Muestra.Alicuotas.Clear();

                AlicuotaRecepcionAguaModel modelo = control.treeAlicuotas.tree.Model as AlicuotaRecepcionAguaModel;

                modelo.Root.Items.ForEach(a =>
                {
                    AlicuotaItem alic = a as AlicuotaItem;
                    if (alic != null)
                    {
                        control.Muestra.Alicuotas.Add(alic.Alicuota);
                    }
                });
            }
        }

        private void FillDatosParametros()
        {
            var controles = listaMuestras.Children.OfType<ControlMuestraRecepAgua>()
                                .Where(c => c.IsEnabled);
            foreach (ControlMuestraRecepAgua control in controles)
            {
                control.Muestra.Parametros.Clear();
                AlicuotaRecepcionAguaModel modelo = control.treeAlicuotas.tree.Model as AlicuotaRecepcionAguaModel;

                modelo.Root.Items.ForEach(a =>
                {
                    AlicuotaItem alic = a as AlicuotaItem;
                    if (alic != null)
                    {
                        alic.Items.ForEach(p =>
                        {
                            ParametroItem param = p as ParametroItem;
                            control.Muestra.Parametros.Add(new LineaAliRecepcionAgua() { IdAlicuota = alic.Id, IdParametro = param.Id });
                        });
                    }
                });
            }

        }

        private void GuardarDatos(NpgsqlConnection conn, int idRecepcion)
        {
            FillDatosAlicuotas();

            var muestras = listaMuestras.Children.OfType<ControlMuestraRecepAgua>()
                                .Where(c => c.IsEnabled)
                                .Select(c => c.Muestra);

            /* muestras agua */
            String consulta = "select insertarmuestrarecepcionagua(@CodigoToma, @Descripcion, @IdRecepcion, @IdPuntoControl, @TieneTomaMuestra)";

            PersistenceDataManipulation.GuardarElement1N(conn, RecepcionAgua, muestras, r => r.Id, "IdRecepcion", consulta);

            /* listas de cada muestra (alicuotas)*/
            PersistenceDataManipulation.GuardarListadoNN(conn, muestras, ma => ma.Alicuotas, ma => ma.Id, "IdMuestra");

            FillDatosParametros();/* una vez añadidos los parametros, para que me asigne bien los ids */

            /* listas de cada alicuota (parametros) */
            foreach (MuestraRecepcionAgua mra in muestras)
            {
                PersistenceDataManipulation.Guardar(conn, mra.Parametros.ToList());
            }
        }

        private void BorrarDatos(NpgsqlConnection conn, int idRecepcion)
        {
            var muestras = listaMuestras.Children.OfType<ControlMuestraRecepAgua>()
                                .Where(c => c.IsEnabled)
                                .Select(c => c.Muestra);

            /* listas de cada alicuota (parametros) */
            foreach (MuestraRecepcionAgua mra in muestras)
            {
                var alicuotas = PersistenceManager.SelectByProperty<AlicuotaRecepcionAgua>("IdMuestra", mra.Id);
                foreach (AlicuotaRecepcionAgua alic in alicuotas)
                {
                    PersistenceDataManipulation.Borrar1N(conn, mra.Parametros.ToList(), alic.Id, "IdAlicuota");
                }
            }

            /* listas de cada muestra (alicuotas)*/
            PersistenceDataManipulation.BorrarListadoNN(conn, muestras, ma => ma.Alicuotas, ma => ma.Id, "IdMuestra");
        }

        private bool ValidarRecepcion()
        {
            FillDatosAlicuotas();
            var muestras = listaMuestras.Children.OfType<ControlMuestraRecepAgua>();
            foreach (ControlMuestraRecepAgua item in muestras)
            {
                if (!item.Validar())
                    return false;
            }

            return panelDatos.GetValidatedInnerValue<RecepcionAgua>() != default(RecepcionAgua);

        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            this.MensajeCancel();
        }
    }
}
