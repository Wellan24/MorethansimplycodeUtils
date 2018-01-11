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
    /// Lógica de interacción para ControlHumedad3Calculo.xaml
    /// </summary>
    public partial class ControlHumedad3Calculo : UserControl
    {
        private Humedad3 humedad;
        public Humedad3 Humedad
        {
            get { return humedad; }
            set
            {
                humedad = value;
                Fill();
            }
        }

        public ControlHumedad3Calculo()
        {
            InitializeComponent();
            CargarHumedad();
        }

        private void CargarHumedad()
        {
            panelCalculos.Build(new Humedad3(),
                new TypePanelSettings<Humedad3>
                {
                    ColumnWidths = new int[] { 1, 1 },
                    Fields = new FieldSettings
                    {
                        ["MediaHumedadTotal2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                                .SetLabel("Valor Medio")
                                .SetReadOnly(true)
                                .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"]),
                        ["Dif2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                                .SetLabel("Dif.")
                                .SetReadOnly(true)
                                .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"]),
                    }
                });
        }

        private void Fill()
        {
            panelCalculos["MediaHumedadTotal2"].SetInnerContent(Calcular.VisualizeDecimals(Humedad.MediaHumedadTotal, 1));
            panelCalculos["Dif2"].SetInnerContent(Calcular.VisualizeDecimals(Humedad.Diferencia, 2));

            labelAceptacion.Aceptacion(Humedad.Aceptado, Name.Equals("CCIAceptacion"));
        }

        public void Clear()
        {
            panelCalculos.Clear();
            labelAceptacion.Visibility = Visibility.Collapsed;
        }
    }
}
