using GenericForms.Abstract;
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
    /// Lógica de interacción para ControlCabeceraPuntoMuestreo.xaml
    /// </summary>
    public partial class ControlCabeceraPuntoMuestreo : UserControl
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

        public ControlCabeceraPuntoMuestreo()
        {
            InitializeComponent();
        }

        private void GenerarDatosPunto()
        {
            panelCabecPuntoMuestreo.Build(PuntoConexion,
                new TypePanelSettings<PuntoConexFocoAtmosfera>
                {
                    ColumnWidths = new int[] { 1 },
                    Fields = new FieldSettings
                    {
                        ["Localizacion"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetLabel("")
                    }
                });
        }
    }
}
