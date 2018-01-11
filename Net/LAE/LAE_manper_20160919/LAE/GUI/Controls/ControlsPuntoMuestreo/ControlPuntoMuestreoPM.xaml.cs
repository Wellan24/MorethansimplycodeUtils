using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using LAE.Modelo;
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
    /// Lógica de interacción para ControlPuntoMuestreoPM.xaml
    /// </summary>
    public partial class ControlPuntoMuestreoPM : UserControl
    {
        private PuntoConexFocoAtmosfera puntoConexion;
        public PuntoConexFocoAtmosfera PuntoConexion
        {
            get { return puntoConexion; }
            set
            {
                puntoConexion = value;
                GenerarDatosPunto();
            }
        }

        public ControlPuntoMuestreoPM()
        {
            InitializeComponent();
        }

        private void GenerarDatosPunto()
        {
            panelPuntoMuestreo.Build(PuntoConexion,
                new TypePanelSettings<PuntoConexFocoAtmosfera>
                {
                    ColumnWidths = new int[] { 5, 1, 5 },
                    Fields = new FieldSettings
                    {
                        ["Velocidad"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetLabel(""),
                        ["Separador"] = PropertyControlSettingsEnum.LabelDefault
                            .SetLabel("/"),
                        ["Temperatura"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetLabel(""),
                        ["AnguloEje"] = PropertyControlSettingsEnum.CheckBoxDefault
                            .SetLabel("")
                            .SetColumnSpan(3)
                            .SetHorizontalAlignment(HorizontalAlignment.Center)
                    }
                });
            
        }
    }
}
