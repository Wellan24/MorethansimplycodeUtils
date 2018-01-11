using GenericForms.Abstract;
using GenericForms.Settings;
using LAE.Calculos;
using LAE.Clases;
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

namespace GUI.Analisis
{
    /// <summary>
    /// Lógica de interacción para ControlCenizasCalculo.xaml
    /// </summary>
    public partial class ControlCenizasCalculo : UserControl
    {
        private Cenizas cenizas;
        public Cenizas Cenizas
        {
            get { return cenizas; }
            set
            {
                cenizas = value;
                Fill();
            }
        }

        public ControlCenizasCalculo()
        {
            InitializeComponent();
            CargarCenizas();
        }

        private void CargarCenizas()
        {
            panelCalculos.Build(new Cenizas(),
                new TypePanelSettings<Cenizas>
                {
                    Fields = new FieldSettings
                    {
                        ["MediaCenizasHumeda2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                                .SetLabel("Media b.h.")
                                .SetReadOnly(true),
                        ["MediaCenizasSeca2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                                .SetLabel("Media b.s.")
                                .SetReadOnly(true),
                        ["Dif2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetReadOnly(true)
                            .SetLabel("Dif."),
                    }
                });
        }

        private void Fill()
        {
            panelCalculos["MediaCenizasHumeda2"].SetInnerContent(Calcular.VisualizeDecimals(Cenizas.MediaCenizasHumeda, 2));
            panelCalculos["MediaCenizasSeca2"].SetInnerContent(Calcular.VisualizeDecimals(Cenizas.MediaCenizasSeca, 2));
            panelCalculos["Dif2"].SetInnerContent(Calcular.VisualizeDecimals(Cenizas.Dif, 2));

            labelAceptacion.Aceptacion(Cenizas.Aceptado);
        }

        public void Clear()
        {
            panelCalculos.Clear();
            labelAceptacion.Visibility = Visibility.Collapsed;
        }
    }
}
