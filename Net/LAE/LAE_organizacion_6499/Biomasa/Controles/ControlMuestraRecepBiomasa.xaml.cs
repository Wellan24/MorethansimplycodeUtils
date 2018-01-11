using Cartif.Extensions;
using GenericForms.Abstract;
using GenericForms.Settings;
using LAE.Biomasa.Modelo;
using LAE.Biomasa.Ventanas;
using LAE.Comun.Modelo;
using LAE.Comun.Modelo.Procedimientos;
using LAE.Comun.Persistence;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LAE.Biomasa.Controles
{
    /// <summary>
    /// Lógica de interacción para ControlMuestraRecepBiomasa.xaml
    /// </summary>
    public partial class ControlMuestraRecepBiomasa : UserControl
    {
        private MuestraRecepcionBiomasa muestra;
        public MuestraRecepcionBiomasa Muestra
        {
            get { return muestra; }
            set
            {
                muestra = value;
                GenerarDatosMuestra();
                CargarParametrosDeterminar();
                if (Muestra.Id>0)
                    botonAnalisis.Visibility = Visibility.Visible;
            }
        }

        public WrapPanel ParametrosDeterminar => this.parametrosDeterminar;

        public ControlMuestraRecepBiomasa()
        {
            InitializeComponent();
            GenerarParametrosDeterminar();
        }

        private void GenerarParametrosDeterminar()
        {
            Procedimiento[] lista = PersistenceManager.SelectAll<Procedimiento>().ToArray();
            foreach (Procedimiento proc in lista)
            {
                CheckBox cb = new CheckBox();
                cb.Content = proc.Siglas;
                cb.Tag = proc.Id;
                cb.Width = 70;
                cb.Height = 20;

                parametrosDeterminar.Children.Add(cb);
            }
        }

        private void GenerarDatosMuestra()
        {
            panelMuestra.Build(Muestra,
                new TypePanelSettings<MuestraRecepcionBiomasa>
                {
                    ColumnWidths = new int[] { 6, 6, 6, 3, 2 },
                    Fields = new FieldSettings
                    {
                        ["Identificacion"] = PropertyControlSettingsEnum.TextBoxDefaultLarge,
                        ["GetCodigoLae"] = PropertyControlSettingsEnum.TextBoxDefaultLarge
                            .SetEnabled(false)
                            .SetLabel("Código"),
                        ["Descripcion"] = PropertyControlSettingsEnum.TextBoxDefaultLarge,
                        ["Cantidad"] = PropertyControlSettingsEnum.TextBoxDefaultLargeNoEmpty
                            .SetLabel("*Cantidad"),
                        ["IdUdsCantidad"] = PropertyControlSettingsEnum.ComboBoxDefaultLargeNoEmpty
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Masa"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["Acreditada"] = PropertyControlSettingsEnum.CheckBoxDefault
                    },
                    IsUpdating = true
                });
            Parametro[] param = FactoriaParametros.GetParametrosByTipo("Biomasa");
            LineaRevisionOferta[] parametros = PersistenceManager.SelectByProperty<LineaRevisionOferta>("IdPControlRevisionOferta", Muestra.IdPuntoControl).ToArray();

            gridParametros.Build(parametros,
                new TypeGridSettings
                {
                    Columns = new ColumnGridSettings
                    {
                        ["IdParametro"] = new TypeGridColumnSettings
                        {
                            Label = "Parámetros oferta",
                            ColumnCombo = new TypeGCComboSettings
                            {
                                InnerValues = param,
                                Path = "Id"
                            }
                        }
                    }
                });
        }

        public bool Validar()
        {
            return panelMuestra.GetValidatedInnerValue<MuestraRecepcionBiomasa>() != default(MuestraRecepcionBiomasa);
        }

        private void CargarParametrosDeterminar()
        {
            ParametroMuestraBiomasa[] pmb = PersistenceManager.SelectByProperty<ParametroMuestraBiomasa>("IdMuestra", Muestra.Id).ToArray();
            parametrosDeterminar.Children.OfType<CheckBox>().ForEach(cb =>
            {
                if (pmb.Any(pr => pr.IdProcedimiento == (int)cb.Tag))
                    cb.IsChecked = true;
            });
        }

        public void MostrarBotonAnalisis()
        {
            if (Muestra.Id > 0)
            {
                botonAnalisis.Visibility = Visibility.Visible;
                /* No es capaz de rellenar el código */
                /*String codigo = PersistenceManager.SelectByID<MuestraRecepcionBiomasa>(Muestra.Id).GetCodigoLae;
                panelMuestra["GetCodigoLae"].SetInnerContent(codigo);*/
            }
        }

        private void VentanaAnalisis_Click(object sender, RoutedEventArgs e)
        {
            PNTsBiomasa ventana = new PNTsBiomasa(Muestra.Id);
            ventana.ShowDialog();
        }
    }
}
