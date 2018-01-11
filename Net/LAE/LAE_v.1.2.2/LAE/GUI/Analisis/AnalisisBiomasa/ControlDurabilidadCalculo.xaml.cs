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
    /// Lógica de interacción para ControlDurabilidadCalculo.xaml
    /// </summary>
    public partial class ControlDurabilidadCalculo : UserControl
    {

        private Finos finos;
        public Finos Finos
        {
            get { return finos; }
            set
            {
                finos = value;
                FillFinos();
            }
        }

        private Durabilidad durabilidad;
        public Durabilidad Durabilidad
        {
            get { return durabilidad; }
            set
            {
                durabilidad = value;
                FillDurabilidad();
            }
        }

        public ControlDurabilidadCalculo()
        {
            InitializeComponent();
            CargarFinos();
            CargarDurabilidad();
        }

        private void CargarFinos()
        {
            panelCalculosFinos.Build(new Finos(),
                new TypePanelSettings<Finos>
                {
                    ColumnWidths=new int[] {1},
                    Fields = new FieldSettings
                    {
                        ["MediaFinos2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("Promedio FIN(%)")
                            .SetReadOnly(true)
                    }
                });
        }

        private void CargarDurabilidad()
        {
            panelCalculosDurabilidad.Build(new Durabilidad(),
                new TypePanelSettings<Durabilidad>
                {
                    Fields = new FieldSettings
                    {
                        ["Durabilidad2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("Promedio DUR(%)")
                            .SetReadOnly(true),
                        ["Dif2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("Dif")
                            .SetReadOnly(true),
                    }, 
                    ColumnWidths=new int[] {1,1}
                });
        }

        private void FillFinos()
        {
            panelCalculosFinos["MediaFinos2"].SetInnerContent(Calcular.VisualizeDecimals(Finos.MediaFinos, 2));

            //labelAceptacionFinos.Aceptacion(Finos.Aceptado);
        }

        private void FillDurabilidad()
        {
            panelCalculosDurabilidad["Durabilidad2"].SetInnerContent(Calcular.VisualizeDecimals(Durabilidad.MediaDurabilidad, 2));
            panelCalculosDurabilidad["Dif2"].SetInnerContent(Calcular.VisualizeDecimals(Durabilidad.Dif, 2));

            labelAceptacionDurabilidad.Aceptacion(Durabilidad.Aceptado);
        }

        public void ClearFinos()
        {
            panelCalculosFinos.Clear();
            //labelAceptacionFinos.Visibility = Visibility.Collapsed;
        }

        public void ClearDurabilidad()
        {
            panelCalculosDurabilidad.Clear();
            labelAceptacionDurabilidad.Visibility = Visibility.Collapsed;
        }
    }
}
