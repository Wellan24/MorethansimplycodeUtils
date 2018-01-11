using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using LAE.Comun.Calculos;
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

namespace LAE.Biomasa.Controles
{
    /// <summary>
    /// Lógica de interacción para ControlInforme.xaml
    /// </summary>
    public partial class ControlInforme : UserControl
    {
        private ResultadoInforme resultado;
        public ResultadoInforme Resultado
        {
            get { return resultado; }
            set
            {
                resultado = value;
                ModificarResultado();
            }
        }

        public ControlInforme()
        {
            InitializeComponent();
            GenerarPanel();
        }

        private void GenerarPanel()
        {
            panelResultado.Build(new ResultadoInforme(),
                new TypePanelSettings<ResultadoInforme>
                {
                    ColumnWidths = new int[] { 1, 1 },
                    Fields = new FieldSettings
                    {
                        ["Resultado"] = PropertyControlSettingsEnum.TextBoxEmptyToNullReadOnly,
                        ["Incertidumbre"] = PropertyControlSettingsEnum.TextBoxEmptyToNullReadOnly,
                    }
                });
        }

        public void ModificarResultado()
        {
            panelResultado["Resultado"].SetInnerContent(Resultado.Valor);
            panelResultado["Incertidumbre"].SetInnerContent(Resultado.Incertidumbre);

            panelResultado["Resultado"].ControlToolTipText = Resultado.Alcance;
            panelResultado["Incertidumbre"].ControlToolTipText = Resultado.RangoIncertidumbre;

        }

    }
}
