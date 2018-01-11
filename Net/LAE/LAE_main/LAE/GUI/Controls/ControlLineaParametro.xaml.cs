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

        private ObservableCollection<ILineasParametros> lineasParametros;
        public ObservableCollection<ILineasParametros> LineasParametros
        {
            get { return lineasParametros; }
            set
            {
                lineasParametros = value;
                ParametrosGrid = RecuperarParametros();
                ParametrosPanel = new Parametro[] { };
                GenerarAddParametros();
            }
        }

        public ControlLineaParametro()
        {
            InitializeComponent();
        }

        private void GenerarAddParametros()
        {
            panelParametros.Build<ILineasParametros>(new ILineasParametros() { Cantidad = 1 },
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
                    PanelValidation = Expectation<ILineasParametros>.Should().AddTest(l => l.Cantidad > 0),
                    DefaultSettings = new PropertyControlSettings
                    {
                        ReadOnly = !Editable,
                    }
                });

            gridParametros.Build<ILineasParametros>(lineasParametros,
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
                                Size = 30,
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
            if (lineasParametros.Count() == 1 && lineasParametros.First().IdParametro == 0)
                lineasParametros.Clear();
        }

        private void NuevoParametro()
        {
            NuevoParametro np = new NuevoParametro();
            np.Owner = Window.GetWindow(this);
            np.ShowDialog();

            if (np.DialogResult ?? false)
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
            lineasParametros.Remove(lineaBorrada);
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
                lineasParametros.Add(lineaParametroAdd);

                panelParametros.InnerValue = new ILineasParametros() { Cantidad = lineaParametroAdd.Cantidad };
            }
            else
            {
                MessageBox.Show("Datos erróneos. Por favor, revisa la información");
            }
        }

    }
}
