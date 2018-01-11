using GenericForms.Abstract;
using GenericForms.Settings;
using GUI.Windows;
using LAE.Modelo;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI.Controls
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
                if(Muestra.Id>0)
                    botonAnalisis.Visibility = Visibility.Visible;
            }
        }

        public ControlMuestraRecepBiomasa()
        {
            InitializeComponent();
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
                            Label = "Parámetros a determinar",
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

        public void MostrarBotonAnalisis()
        {
            if (Muestra.Id > 0)
            {
                botonAnalisis.Visibility = Visibility.Visible;
                /* No es capaz de rellenar el código */
                //String codigo = PersistenceManager.SelectByID<MuestraRecepcionBiomasa>(Muestra.Id).GetCodigoLae;
                //panelMuestra["GetCodigoLae"].SetInnerContent(codigo);
            }
        }

        private void VentanaAnalisis_Click(object sender, RoutedEventArgs e)
        {
            PNTsBiomasa ventana = new PNTsBiomasa(Muestra.Id);
            ventana.ShowDialog();
        }
    }
}
