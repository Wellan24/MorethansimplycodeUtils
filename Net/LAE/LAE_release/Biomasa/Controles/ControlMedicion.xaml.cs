using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using LAE.Comun.Modelo;
using LAE.Comun.Modelo.Procedimientos;
using LAE.Biomasa.Modelo;
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
    /// Lógica de interacción para ControlMedicion.xaml
    /// </summary>
    public partial class ControlMedicion : UserControl
    {
        private MedicionPNT medicion;
        public MedicionPNT Medicion
        {
            get { return medicion; }
            set
            {
                if (medicion == null)
                {
                    medicion = value;
                    GenerarPanelMedicion();
                }
                else
                    medicion = value;
            }
        }

        public ControlMedicion()
        {
            InitializeComponent();
        }

        private void GenerarPanelMedicion()
        {
            panelMedicion.Build(Medicion,
            new TypePanelSettings<MedicionPNT>
            {
                ColumnWidths = new int[] { 2, 2, 1, 2 },
                Fields = new FieldSettings
                {
                    ["FechaInicio"] = PropertyControlSettingsEnum.DateTimeDefaultNoEmpty
                        .SetLabel("* Fecha análisis"),
                    ["IdTecnico"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                        .SetLabel("* Técnico")
                        .SetInnerValues(FactoriaTecnicos.GetTecnicos()),
                    ["Finalizado"] = PropertyControlSettingsEnum.CheckBoxDefault,
                    ["Observaciones"] = PropertyControlSettingsEnum.TextBoxDefault
                        .SetHeightMultiline(45)
                },
                IsUpdating = true
            });
        }

    }
}
