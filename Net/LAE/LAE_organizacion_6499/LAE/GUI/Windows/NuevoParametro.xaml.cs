using Cartif.Extensions;
using Cartif.XamlResources;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using LAE.Comun.Clases;
using LAE.Modelo;
using MahApps.Metro.Controls;
using LAE.Comun.Persistence;
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
using System.Windows.Shapes;
using LAE.Comun.Modelo;

namespace LAE.GUI.Windows
{
    /// <summary>
    /// Lógica de interacción para NuevoParametro.xaml
    /// </summary>
    public partial class NuevoParametro : MetroWindow
    {
        public static readonly DependencyProperty IconTitleProperty =
            DependencyProperty.Register("IconTitle", typeof(ImageSource), typeof(NuevoParametro), new PropertyMetadata(null));
        public ImageSource IconTitle
        {
            get { return (ImageSource)GetValue(IconTitleProperty); }
            set { SetValue(IconTitleProperty, value); }
        }

        public ObservableCollection<Parametro> LineasParametro { get; set; }

        public NuevoParametro()
        {
            InitializeComponent();
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE;component/images/cabecera.png", UriKind.Absolute));
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

            LineasParametro = new ObservableCollection<Parametro>() { new Parametro()};
            GenerarPanelParametro();
            GenerarGridParametro();
        }

        private void GenerarPanelParametro()
        {
            panelParametro.Build(new Parametro(),
                new TypePanelSettings<Parametro>
                {
                    Fields = new FieldSettings
                    {
                        ["NombreParametro"] = PropertyControlSettingsEnum.TextBoxDefaultNoEmpty
                                .SetLabel("Parámetro"),
                        ["MetodoParametro"] = PropertyControlSettingsEnum.TextBoxDefault
                                .SetLabel("Método"),
                        ["Norma"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["Conservacion"] = PropertyControlSettingsEnum.TextBoxDefault
                                .SetLabel("Conservación"),
                        ["IdTipoMuestra"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                .SetInnerValues(CargarTipoMuestra())
                                .SetLabel("Tipo de muestra"),
                        ["BotonAdd"] = new PropertyControlSettings
                        {
                            Type = typeof(PropertyControlButton),
                            DesignPath = CSVPath.ArrowDownIcon,
                            Click = (sender, e) =>
                            {
                                if (panelParametro.GetValidatedInnerValue<Parametro>() != default(Parametro))
                                {
                                    Parametro parametroAdd = panelParametro.InnerValue.Clone(typeof(Parametro)) as Parametro;
                                    LineasParametro.Add(parametroAdd);
                                    panelParametro.InnerValue = new Parametro();
                                }
                                else
                                {
                                    MessageBox.Show("Datos erróneos. Por favor, revisa la información");
                                }
                            }
                        },
                    }
                });
        }

        private void GenerarGridParametro()
        {
            gridParametro.Build(LineasParametro,
                new TypeGridSettings
                {
                    Columns = new ColumnGridSettings
                    {
                        ["NombreParametro"] = TypeGridColumnSettingsEnum.DefaultColum.
                                    SetLabel("Parámetro"),
                        ["MetodoParametro"] = TypeGridColumnSettingsEnum.DefaultColum.
                                    SetLabel("Método"),
                        ["IdTipoMuestra"] = new TypeGridColumnSettings
                        {
                            Label = "Tipo de muestra",
                            Width = 2,
                            ColumnCombo = new TypeGCComboSettings
                            {
                                InnerValues = CargarTipoMuestra(),
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
                                Size = 25,
                                Margin = 4,
                                Click = (sender, e) =>
                                {
                                    DeleteTipoMuestra(sender);
                                },
                            }
                        }
                        .SetWidth(1),
                    }
                });
            LineasParametro.Clear();
        }

        private void DeleteTipoMuestra(object sender)
        {
            Parametro lineaBorrada = ((FrameworkElement)sender).DataContext as Parametro;
            LineasParametro.Remove(lineaBorrada);
        }

        private TipoMuestra[] CargarTipoMuestra()
        {
            return PersistenceManager.SelectAll<TipoMuestra>().OrderBy(t => t.Nombre).ToArray();
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void bGuardar_Click(object sender, RoutedEventArgs e)
        {
            LineasParametro.ForEach(l => {
                int idParametro = l.Insert();
                l.Id = idParametro;
            });

            DialogResult = true;
            this.Close();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Util.VisualizeWindow(this);
        }
    }
}
