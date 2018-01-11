using Cartif.Extensions;
using Cartif.Logs;
using GenericForms.Abstract;
using GenericForms.Settings;
using GUI.Controls;
using LAE.Clases;
using LAE.Modelo;
using MahApps.Metro.Controls;
using Npgsql;
using Persistence;
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
using System.Windows.Shapes;

namespace GUI.Windows
{
    /// <summary>
    /// Lógica de interacción para RecepcionesMuestraBiomasa.xaml
    /// </summary>
    public partial class RecepcionesMuestraBiomasa : MetroWindow
    {
        public static readonly DependencyProperty IconTitleProperty =
            DependencyProperty.Register("IconTitle", typeof(ImageSource), typeof(RecepcionesMuestraBiomasa), new PropertyMetadata(null));
        public ImageSource IconTitle
        {
            get { return (ImageSource)GetValue(IconTitleProperty); }
            set { SetValue(IconTitleProperty, value); }
        }

        private RecepcionBiomasa recepcionBiomasa;
        public RecepcionBiomasa RecepcionBiomasa
        {
            get { return recepcionBiomasa; }
            set
            {
                recepcionBiomasa = value;
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

        public RecepcionesMuestraBiomasa(int idCliente, int idContacto, RecepcionBiomasa recepcion)
        {
            InitializeComponent();
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE;component/images/cabecera.png", UriKind.Absolute));
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

            RecepcionBiomasa = recepcion;
            IdCliente = idCliente;
            IdContacto = idContacto;
        }

        private void CargarDatos()
        {
            GenerarDatosMuestras();
            if (RecepcionBiomasa?.Id != 0)
                CargarMuestra();
            else
                GenerarMuestra();
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
            panelDatos.Build(RecepcionBiomasa,
                new TypePanelSettings<RecepcionBiomasa>
                {
                    Fields = new FieldSettings
                    {
                        ["FechaRecepcion"] = PropertyControlSettingsEnum.DateTimeDefaultNoEmpty
                                        .SetLabel("Fecha recepción*"),
                        ["IdTecnico"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                        .SetInnerValues(FactoriaTecnicos.GetTecnicos())
                                        .SetLabel("Técnico"),
                    },
                    IsUpdating = true
                });

            panelObservaciones.Build(RecepcionBiomasa,
                new TypePanelSettings<RecepcionBiomasa>
                {
                    ColumnWidths = new int[] { 1 },
                    Fields = new FieldSettings
                    {
                        ["Observaciones"] = PropertyControlSettingsEnum.TextBoxDefault
                                .SetHeightMultiline(60)

                    },
                    IsUpdating=true
                });
        }

        private void CargarMuestra()
        {

            MuestraRecepcionBiomasa[] muestras = PersistenceManager.SelectByProperty<MuestraRecepcionBiomasa>("IdRecepcion", RecepcionBiomasa.Id).ToArray();
            muestras.ForEach(m =>
            {
                ControlMuestraRecepBiomasa cmrb = new ControlMuestraRecepBiomasa() { Muestra = m };
                listaMuestras.Children.Add(cmrb);
            });
        }

        private void GenerarMuestra()
        {
            PuntocontrolRevision[] puntosControl = FactoriaPuntocontrolRevision.GetPuntosControl(RecepcionBiomasa.IdTrabajo);

            puntosControl.ForEach(pc =>
            {
                ControlMuestraRecepBiomasa cmrb = new ControlMuestraRecepBiomasa() { Muestra = new MuestraRecepcionBiomasa() { IdPuntoControl = pc.Id } };
                listaMuestras.Children.Add(cmrb);
            });
        }

        private void bGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarRecepcion())
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                using (NpgsqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        int n=PersistenceDataManipulation.Guardar(conn, RecepcionBiomasa);
                        /* muestras biomasa */
                        String consulta = "select insertarmuestrarecepcionbiomasa(@Identificacion, @Descripcion, @Cantidad, @IdUdsCantidad, @Acreditada, @IdRecepcion, @IdPuntoControl)";
                        
                        var muestras = listaMuestras.Children.OfType<ControlMuestraRecepBiomasa>().Select(c=>c.Muestra);
                        PersistenceDataManipulation.GuardarElement1N(conn, RecepcionBiomasa, muestras, r => r.Id, "IdRecepcion", consulta);
                        listaMuestras.Children.OfType<ControlMuestraRecepBiomasa>().ForEach(cm => cm.GenerarBotonAnalisis());
                        
                            trans.Commit();
                        MessageBox.Show("Datos guardados con éxito");
                    }
                    catch (Exception ex)
                    {
                        if (trans != null)
                            trans.Rollback();

                        CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "Error al guardar la recepción e inspección de muestra (biomasa)", ex);
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

        private bool ValidarRecepcion()
        {
            var muestras = listaMuestras.Children.OfType<ControlMuestraRecepBiomasa>();
            foreach (ControlMuestraRecepBiomasa item in muestras)
            {
                if (!item.Validar())
                    return false;
            }
            return panelDatos.GetValidatedInnerValue<RecepcionBiomasa>() != default(RecepcionBiomasa);
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            this.MensajeCancel();
        }
    }
}
