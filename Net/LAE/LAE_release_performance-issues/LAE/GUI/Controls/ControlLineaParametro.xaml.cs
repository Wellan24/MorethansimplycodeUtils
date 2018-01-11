using Cartif.Expectation;
using Cartif.Extensions;
using Cartif.Util;
using Cartif.XamlResources;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using LAE.GUI.Windows;
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
        public bool? editable;
        public bool Editable
        {
            get { return editable ?? true; }
            set
            {
                editable = value;
            }
        }

        public Action<ControlLineaParametro> DeleteControl { get; set; }

        public Parametro[] ParametrosGrid;
        public Parametro[] ParametrosPanel;
        private int[] tiposMuestraSeleccionados;
        public int[] TiposMuestraSeleccionados
        {
            get { return tiposMuestraSeleccionados; }
            set
            {
                tiposMuestraSeleccionados = value;
                RecuperarParametrosPanel();
            }
        }

        private IPuntoControl puntoControl;
        public IPuntoControl PuntoControl
        {
            get { return puntoControl; }
            set
            {
                puntoControl = value;
                GenerarPanelPuntoControl();
                ParametrosGrid = RecuperarParametros();
                ParametrosPanel = new Parametro[] { };
                GenerarAddParametros();
            }
        }

        public ControlLineaParametro()
        {
            InitializeComponent();
        }

        private void GenerarPanelPuntoControl()
        {
            panelPControlNombre.Build(PuntoControl,
                new TypePanelSettings<IPuntoControl>
                {
                    Fields = new FieldSettings
                    {
                        ["Nombre"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetLabel("Nombre punto control/foco"),
                        ["BotonDelete"] = new PropertyControlSettings
                        {
                            Type = typeof(PropertyControlButton),
                            DesignPath = CSVPath.RemoveIcon,
                            Click = (sender, e) => { DeletePuntoControl(sender, e); },
                            BackgroundColor = new SolidColorBrush(Color.FromArgb(255, 181, 0, 0))
                        },
                    },
                    IsUpdating = true
                });

            panelPControl.Build(PuntoControl,
                new TypePanelSettings<IPuntoControl>
                {
                    Fields = new FieldSettings
                    {
                        ["Importe"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["Observaciones"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetHeightMultiline(90)
                            .SetColumnSpan(2),
                    },
                    IsUpdating = true
                });
        }

        private void DeletePuntoControl(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Estás seguro que deseas eliminar el punto de control/foco? Si al finalizar la edición guarda los cambios el punto de control/foco será borrado", "Borrar punto de control", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                DeleteControl(this);
            }
        }

        private void GenerarAddParametros()
        {
            panelParametros.Build(new ILineasParametros() { Cantidad = 1 },
                new TypePanelSettings<ILineasParametros>
                {
                    ColumnWidths = new int[] { 3, 3, 1, 1 },
                    Fields = new FieldSettings
                    {
                        ["Cantidad"] = PropertyControlSettingsEnum.TextBoxDefaultNoEmpty,
                        ["IdParametro"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                        .SetInnerValues(ParametrosPanel)
                                        .SetLabel("Parámetro")
                                        .AddKeyDownCombo(
                                            (sender, e) => { if (e.Key == Key.Enter) AddLineaParametro(); }
                                        ),
                        ["BotonAdd"] = new PropertyControlSettings
                        {
                            Type = typeof(PropertyControlButton),
                            DesignPath = CSVPath.ArrowDownIcon,
                            Click = (sender, e) => { AddLineaParametro(); }
                        },
                        ["BotonCreate"] = new PropertyControlSettings
                        {
                            Type = typeof(PropertyControlButton),
                            DesignPath = CSVPath.PlusIcon,
                            Click = (sender, e) => { NuevoParametro(); }
                        },
                    },
                    PanelValidation = Expectation<ILineasParametros>.ShouldBe().AddCriteria(l => l.Cantidad > 0),
                    DefaultSettings = new PropertyControlSettings
                    {
                        ReadOnly = !Editable,
                    }
                });

            gridParametros.Build(PuntoControl.Lineas,
                new TypeGridSettings
                {
                    Columns = new ColumnGridSettings
                    {
                        ["Cantidad"] = TypeGridColumnSettingsEnum.DefaultColum,
                        ["IdParametro"] = new TypeGridColumnSettings
                        {
                            Label = "Parámetro (Método)", /* sirve de id, cuidado al cambiarlo */
                            Width = 2,
                            ColumnCombo = new TypeGCComboSettings
                            {
                                InnerValues = ParametrosGrid,
                                Path = "Id",
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
                                    DeleteParametro(sender);
                                },
                                Enabled = Editable,
                            }
                        }
                        .SetWidth(1),
                    }
                }
                );
            if (PuntoControl.Lineas.Count() == 1 && PuntoControl.Lineas.First().IdParametro == 0)
                PuntoControl.Lineas.Clear();


        }

        private void NuevoParametro()
        {
            NuevoParametro np = new NuevoParametro();
            np.Owner = Window.GetWindow(this);
            np.ShowDialog();

            if (np.DialogResult == true)
            {
                np.LineasParametro.ForEach(l => ParametrosGrid = ParametrosGrid.Insert(l));
                Array.Sort(ParametrosGrid);

                ((DataGridComboBoxColumn)gridParametros["Parámetro (Método)"]).ItemsSource = ParametrosGrid;
                RecuperarParametrosPanel();
            }
        }

        private void DeleteParametro(object sender)
        {
            ILineasParametros lineaBorrada = ((FrameworkElement)sender).DataContext as ILineasParametros;
            PuntoControl.Lineas.Remove(lineaBorrada);
        }

        private Parametro[] RecuperarParametros()
        {
            return PersistenceManager.SelectAll<Parametro>().OrderBy(t => t.NombreParametro).ToArray();
        }

        public void RecuperarParametrosPanel()
        {

            if (TiposMuestraSeleccionados != null)
            {
                Parametro[] lista = new Parametro[] { };
                TiposMuestraSeleccionados.ForEach(id =>
                {
                    lista = lista.Insert(PersistenceManager.SelectByProperty<Parametro>("IdTipoMuestra", id)
                                                            .OrderBy(t => t.NombreParametro).ToArray()
                                         );
                });
                Array.Sort(lista);
                ParametrosPanel = lista;
                panelParametros["IdParametro"].InnerValues = ParametrosPanel;
            }

        }

        private void AddLineaParametro()
        {
            if (panelParametros.GetValidatedInnerValue<ILineasParametros>() != default(ILineasParametros))
            {
                ILineasParametros lineaParametroAdd = panelParametros.InnerValue.Clone(typeof(ILineasParametros)) as ILineasParametros;
                PuntoControl.Lineas.Add(lineaParametroAdd);

                panelParametros.InnerValue = new ILineasParametros() { Cantidad = lineaParametroAdd.Cantidad };
            }
            else
            {
                MessageBox.Show("Datos erróneos. Por favor, revisa la información");
            }
        }

    }
}
