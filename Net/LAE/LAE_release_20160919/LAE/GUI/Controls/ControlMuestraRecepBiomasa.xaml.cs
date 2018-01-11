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
                GenerarBotonAnalisis();
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
                            .SetLabel(""),
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

        public void GenerarBotonAnalisis()
        {
            if (Muestra.Id > 0)
            {
                Button btn = new Button() { Content = "Realizar análisis" };
                btn.Click += (s, e) => AbrirVentanaAnalisis();
                stack.Children.Add(btn);
            }
        }

        private void AbrirVentanaAnalisis()
        {
            PNTsBiomasa ventana = new PNTsBiomasa(Muestra.Id);
            ventana.ShowDialog();
        }
    }
}
