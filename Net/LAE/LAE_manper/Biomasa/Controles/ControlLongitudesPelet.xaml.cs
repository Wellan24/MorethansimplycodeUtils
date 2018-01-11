using Cartif.Expectation;
using Cartif.Extensions;
using Cartif.XamlResources;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using LAE.Comun.Modelo;
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
    /// Lógica de interacción para ControlLongitudesPelet.xaml
    /// </summary>
    public partial class ControlLongitudesPelet : UserControl
    {
        private Label labelSelected = null;

        private ClasePelet clase;
        public ClasePelet Clase
        {
            get { return clase; }
            set
            {
                clase = value;
                GenerarLongitudes();
            }
        }

        public List<LongitudPelet> Longitudes
        {
            get { return listaLongitudes.Children.OfType<Label>().Select(l => l.Tag as LongitudPelet).ToList(); }
        }

        public Action UpdateData { get; set; }

        public ControlLongitudesPelet()
        {
            InitializeComponent();
            GenerarAddLongPelet();
        }

        private void GenerarAddLongPelet()
        {
            int idMilimetros = Unidad.Of("Milimetros").Id;

            panelAdd.Build(new LongitudPelet() { IdUdsMedida = idMilimetros },
                new TypePanelSettings<LongitudPelet>
                {
                    ColumnWidths = new int[] { 2, 1, 1, 1 },
                    Fields = new FieldSettings
                    {
                        ["Medida"] = PropertyControlSettingsEnum.TextBoxDefault
                            .AddKeyDown(EnterLongitud)
                            .SetLabel("Longitud"),
                        ["IdUdsMedida"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Longitud"))
                            .SetLabel("")
                            .SetDisplayMemberPath("Abreviatura")
                            .SetReadOnly(true),
                        ["BotonAdd"] = new PropertyControlSettings
                        {
                            Type = typeof(PropertyControlButton),
                            DesignPath = CSVPath.ArrowRightIcon,
                            Click = (sender, e) => { AddLongitud(); }
                        },
                        ["BotonDelete"] = new PropertyControlSettings
                        {
                            Type = typeof(PropertyControlButton),
                            DesignPath = CSVPath.RemoveIcon,
                            Click = (sender, e) => { DeleteLongitud(); }
                        },

                    },
                    PanelValidation = Expectation<LongitudPelet>.ShouldBe().AddCriteria(lp => lp.Medida > 0)
                });
        }

        private void GenerarLongitudes()
        {
            foreach (LongitudPelet longitud in Clase.Longitudes)
            {
                Label l = CrearLabelLongitud(longitud);
                listaLongitudes.Children.Add(l);
            }
        }

        private void EnterLongitud(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AddLongitud();
        }

        private void DeleteLongitud()
        {
            if (labelSelected != null)
            {
                int n = listaLongitudes.Children.IndexOf(labelSelected);
                Clase.Longitudes.RemoveAt(n);

                listaLongitudes.Children.Remove(labelSelected);
                panelAdd.InnerValue = new LongitudPelet() { IdUdsMedida = 14 };
                labelSelected = null;
                UpdateData();
            }
        }

        private void AddLongitud()
        {

            if (panelAdd.GetValidatedInnerValue<LongitudPelet>() != default(LongitudPelet))
            {
                LongitudPelet longPelet = panelAdd.InnerValue.Clone(typeof(LongitudPelet)) as LongitudPelet;
                if (labelSelected != null)
                {
                    labelSelected.Content = (longPelet.Medida ?? 0).ToString("F1");
                    labelSelected.Tag = longPelet;

                    int n=listaLongitudes.Children.IndexOf(labelSelected);
                    Clase.Longitudes[n] = longPelet;
                }
                else
                {
                    Label label = CrearLabelLongitud(longPelet);
                    listaLongitudes.Children.Add(label);
                    panelAdd.InnerValue = new LongitudPelet() { IdUdsMedida = 14 };
                    Clase.Longitudes.Add(longPelet);
                }
                UpdateData();
            }
            else
            {
                MessageBox.Show("Datos erróneos. Por favor, revisa la información");
            }
        }

        private Label CrearLabelLongitud(LongitudPelet l)
        {
            Label label = new Label();
            label.Width = 100;
            label.BorderBrush = new SolidColorBrush(Colors.LightGray);
            label.BorderThickness = new Thickness(0.5);
            label.Content = (l.Medida ?? 0).ToString("F1");
            label.Tag = l;
            label.MouseLeftButtonUp += new MouseButtonEventHandler(SeleccionLongitud);

            label.Style = (Style)FindResource("labelHand");
            return label;
        }

        private void SeleccionLongitud(object sender, MouseButtonEventArgs e)
        {
            Label label = sender as Label;

            if (label == labelSelected)
            {
                label.Background = new SolidColorBrush(Colors.White);
                panelAdd.InnerValue = new LongitudPelet() { IdUdsMedida = 14 };
                labelSelected = null;
            }
            else
            {
                if (labelSelected != null)
                    labelSelected.Background = new SolidColorBrush(Colors.White);

                label.Background = new SolidColorBrush(Color.FromArgb(255, 232, 232, 232));
                panelAdd.InnerValue = label.Tag;

                labelSelected = label;
            }

        }
    }
}
