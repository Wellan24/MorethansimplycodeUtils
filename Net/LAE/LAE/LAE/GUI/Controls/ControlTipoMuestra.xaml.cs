using Cartif.Extensions;
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
    /// Lógica de interacción para ControlTipoMuestra.xaml
    /// </summary>
    public partial class ControlTipoMuestra : UserControl
    {

        private ObservableCollection<ITipoMuestra> lineasTipoMuestra;
        public ObservableCollection<ITipoMuestra> LineasTipoMuestra
        {
            get { return lineasTipoMuestra; }
            set
            {
                lineasTipoMuestra = value;
                GenerarAddTipoMuestra();
            }
        }


        public ControlTipoMuestra()
        {
            InitializeComponent();
        }


        private void GenerarAddTipoMuestra()
        {
            panelTipoMuestra.Build<ITipoMuestra>(new ITipoMuestra(),
                new TypePanelSettings<ITipoMuestra>
                {
                    ColumnWidths = new int[] { 1 },
                    Fields = new FieldSettings
                    {
                        ["IdTipoMuestra"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                        .SetInnerValues(RecuperarTipoMuestra())
                                        .SetLabel("Tipo de muestra")
                    }
                }
                );

            gridTipoMuestra.Build<ITipoMuestra>(lineasTipoMuestra,
                new TypeGridSettings
                {
                    Columns = new ColumnGridSettings
                    {
                        ["IdTipoMuestra"] = new TypeGridColumnSettings
                        {
                            Label = "Tipo de muestra",
                            Width = 2,
                            ColumnCombo = new TypeGCComboSettings
                            {
                                InnerValues = RecuperarTipoMuestra(),
                                Path = "Id",
                                DisplayPath = "Nombre"
                            }
                        },
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
                                    DeleteTipoMuestra(sender);
                                }
                            }
                        }
                        .SetWidth(1),
                    }
                }
                );
            lineasTipoMuestra.Clear();
        }

        private void addTipoMuestra_Click(object sender, RoutedEventArgs e)
        {
            if (panelTipoMuestra.GetValidatedInnerValue<ITipoMuestra>() != default(ITipoMuestra))
            {

                lineasTipoMuestra.Add(panelTipoMuestra.InnerValue.Clone(typeof(ITipoMuestra)) as ITipoMuestra);
                panelTipoMuestra.InnerValue = new ITipoMuestra();
            }
            else
            {
                MessageBox.Show("Algún dato es erroneo");
            }
        }

        private TipoMuestra[] RecuperarTipoMuestra()
        {
            return PersistenceManager<TipoMuestra>.SelectAll().OrderBy(t => t.Nombre).ToArray();
        }

        private void DeleteTipoMuestra(object sender)
        {
            ITipoMuestra lineaBorrada = ((FrameworkElement)sender).DataContext as ITipoMuestra;
            lineasTipoMuestra.Remove(lineaBorrada);
        }

    }
}
