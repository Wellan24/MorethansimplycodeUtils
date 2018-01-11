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
    /// Lógica de interacción para ControlDensidadCalculo.xaml
    /// </summary>
    public partial class ControlDensidadCalculo : UserControl
    {
        private Densidad densidad;
        public Densidad Densidad
        {
            get { return densidad; }
            set
            {
                densidad = value;
                Fill();
            }
        }

        public ControlDensidadCalculo()
        {
            InitializeComponent();
            CargarDensidad();
        }

        private void CargarDensidad()
        {
            panelCalculos.Build(new Densidad(),
                new TypePanelSettings<Densidad>
                {
                    Fields = new FieldSettings
                    {
                        ["MediaDensidadHumeda2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("Media b.h.")
                            .SetReadOnly(true)
                            .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"]),
                        ["MediaDensidadSeca2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("Media b.s.")
                            .SetReadOnly(true)
                            .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"]),
                        ["Dif2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetReadOnly(true)
                            .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"])
                            .SetLabel("Dif."),
                    }
                });
        }

        private void Fill()
        {
            panelCalculos["MediaDensidadHumeda2"].SetInnerContent(Calcular.VisualizeDecimals(Densidad.MediaDensidadHumeda, 0));
            panelCalculos["MediaDensidadSeca2"].SetInnerContent(Calcular.VisualizeDecimals(Densidad.MediaDensidadSeca, 0));
            panelCalculos["Dif2"].SetInnerContent(Calcular.VisualizeDecimals(Densidad.Dif, 3));

            labelAceptacion.Aceptacion(Densidad.Aceptado, Name.Equals("CCIAceptacion"));
        }

        public void Clear()
        {
            panelCalculos.Clear();
            labelAceptacion.Visibility = Visibility.Collapsed;
        }
    }
}
