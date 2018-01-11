using GenericForms.Abstract;
using GenericForms.Settings;
using LAE.Comun.Calculos;
using LAE.Comun.Clases;
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
                    ColumnWidths = new int[] { 1, 1, 1, 1 },
                    Fields = new FieldSettings
                    {
                        ["MediaCenizasHU3_2"] = PropertyControlSettingsEnum.TextBoxEmptyToNullReadOnly
                                .SetLabel("Media b.h.(HUM3)"),
                        ["MediaCenizasSeca2"] = PropertyControlSettingsEnum.TextBoxEmptyToNullReadOnly
                                .SetLabel("Media b.s."),

                        ["MediaCenizasHUM_2"] = PropertyControlSettingsEnum.TextBoxEmptyToNullReadOnly
                                .SetLabel("Media b.h.(HUM)"),
                        ["Dif2"] = PropertyControlSettingsEnum.TextBoxEmptyToNullReadOnly
                                .SetLabel("Dif."),
                    }
                });
        }

        private void Fill()
        {
            panelCalculos["MediaCenizasHU3_2"].SetInnerContent(Calcular.VisualizeDecimals(Cenizas.MediaCenizasHU3, 2));
            panelCalculos["MediaCenizasSeca2"].SetInnerContent(Calcular.VisualizeDecimals(Cenizas.MediaCenizasSeca, 2));
            panelCalculos["MediaCenizasHUM_2"].SetInnerContent(Calcular.VisualizeDecimals(Cenizas.MediaCenizasHUM, 2));
            panelCalculos["Dif2"].SetInnerContent(Calcular.VisualizeDecimals(Cenizas.Dif, 2));

            labelAceptacion.Aceptacion(Cenizas.Aceptado, Name.Equals("CCIAceptacion"));
        }

        public void Clear()
        {
            panelCalculos.Clear();
            labelAceptacion.Visibility = Visibility.Collapsed;
        }
    }
}
