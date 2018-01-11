using Cartif.Extensions;
using Cartif.Util;
using Cartif.XamlResources;
using GenericForms.Abstract;
using GenericForms.Settings;
using LAE.Modelo;
using Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Lógica de interacción para ControlLineaParametro.xaml
    /// </summary>
    public partial class ControlLineaParametro : UserControl
    {

        private ObservableCollection<ILineasParametros> lineasParametros;
        public ObservableCollection<ILineasParametros> LineasParametros
        {
            get { return lineasParametros; }
            set
            {
                lineasParametros = value;
                GenerarAddParametros();
            }
        }

        public ControlLineaParametro()
        {
            InitializeComponent();
        }

        private void GenerarAddParametros()
        {
            panelParametros.Build<ILineasParametros>(new ILineasParametros(),
                new TypePanelSettings<ILineasParametros>
                {
                    Fields = new FieldSettings
                    {
                        ["Cantidad"] = PropertyControlSettingsEnum.TextBoxDefaultNoEmpty,
                        ["IdParametro"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                        .SetInnerValues(RecuperarParametros())
                                        .SetLabel("Parámetro"),
                        ["Metodo"] = PropertyControlSettingsEnum.TextBoxDefault,
                    },
                    PanelValidation = Expectation<ILineasParametros>.Should().AddTest(l => l.Cantidad > 0)
                }
                );

            gridParametros.Build<ILineasParametros>(lineasParametros,
                new TypeGridSettings
                {
                    Columns = new ColumnGridSettings
                    {
                        ["Cantidad"] = TypeGridColumnSettingsEnum.DefaultColum,
                        ["IdParametro"] = new TypeGridColumnSettings
                        {
                            Label = "Parámetro",
                            Width = 2,
                            ColumnCombo = new TypeGCComboSettings
                            {
                                InnerValues = RecuperarParametros(),
                                Path = "Id",
                                DisplayPath = "NombreParametro"
                            }
                        },
                        ["Metodo"] = TypeGridColumnSettingsEnum.DefaultColum,
                        ["Borrar"] = new TypeGridColumnSettings
                        {
                            ColumnButton = new TypeGCButtonSettings
                            {
                                DesingPath = CSVPath.RemoveIcon,
                                Color = Colors.White,
                                Size = 30,
                                Margin = 4,
                                Click = (sender, e) =>
                                {
                                    DeleteParametro(sender);
                                }
                            }
                        }
                        .SetWidth(1),
                    }
                }
                );
            lineasParametros.Clear();
        }

        private void addTParametro_Click(object sender, RoutedEventArgs e)
        {
            if (panelParametros.GetValidatedInnerValue<ILineasParametros>() != default(ILineasParametros))
            {
                lineasParametros.Add(panelParametros.InnerValue.Clone(typeof(ILineasParametros)) as ILineasParametros);
                panelParametros.InnerValue = new ILineasParametros();
            }
            else
            {
                MessageBox.Show("Algún dato es erroneo");
            }
        }

        private void DeleteParametro(object sender)
        {
            ILineasParametros lineaBorrada = ((FrameworkElement)sender).DataContext as ILineasParametros;
            lineasParametros.Remove(lineaBorrada);
        }

        private Parametro[] RecuperarParametros()
        {
            return PersistenceManager<Parametro>.SelectAll().OrderBy(t => t.NombreParametro).ToArray();
        }


    }
}
