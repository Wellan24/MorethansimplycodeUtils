using Cartif.Extensions;
using Cartif.XamlResources;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using GUI.Windows;
using LAE.Modelo;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LAE.Comun.Modelo;

namespace GUI.Controls
{
    /// <summary>
    /// Lógica de interacción para ControlTipoMuestra.xaml
    /// </summary>
    public partial class ControlTipoMuestra : UserControl
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

        public TipoMuestra[] TiposMuestra;

        private ObservableCollection<ITipoMuestra> lineasTipoMuestra;
        public ObservableCollection<ITipoMuestra> LineasTipoMuestra
        {
            get { return lineasTipoMuestra; }
            set
            {
                lineasTipoMuestra = value;
                TiposMuestra = RecuperarTipoMuestra();
                GenerarAddTipoMuestra();
            }
        }

        public Action<int[]> ActualizarComboParametros { get; set; }
        public Func<int, bool> CanDeleteTipoMuestra { get; set; }

        public ControlTipoMuestra()
        {
            InitializeComponent();
        }


        private void GenerarAddTipoMuestra()
        {
            panelTipoMuestra.Build(new ITipoMuestra(),
                new TypePanelSettings<ITipoMuestra>
                {
                    ColumnWidths = new int[] { 3, 1, 1 },
                    Fields = new FieldSettings
                    {
                        ["IdTipoMuestra"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                        .SetInnerValues(TiposMuestra)
                                        .SetLabel("Tipo de muestra")
                                        .AddKeyDownCombo(
                                            (sender, e) => { if (e.Key == Key.Enter) AddTipoMuestra(); }
                                        ),
                        ["BotonAdd"] = new PropertyControlSettings
                        {
                            Type = typeof(PropertyControlButton),
                            DesignPath = CSVPath.ArrowRightIcon,
                            Click = (sender, e) => { AddTipoMuestra(); }
                        },
                        ["BotonCreate"] = new PropertyControlSettings
                        {
                            Type = typeof(PropertyControlButton),
                            DesignPath = CSVPath.PlusIcon,
                            Click = (sender, e) => { NuevoTipoMuestra(); }
                        },
                    },
                    DefaultSettings = new PropertyControlSettings
                    {
                        ReadOnly = !Editable,
                    }
                }
                );

            gridTipoMuestra.Build(lineasTipoMuestra,
                new TypeGridSettings
                {
                    Columns = new ColumnGridSettings
                    {
                        ["IdTipoMuestra"] = new TypeGridColumnSettings
                        {
                            Label = "Tipo de muestra", /* sirve de id, cuidado al cambiarlo */
                            Width = 2,
                            ColumnCombo = new TypeGCComboSettings
                            {
                                InnerValues = TiposMuestra,
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
                                Enabled = Editable
                            }
                        }
                        .SetWidth(1),
                    }
                }
                );
            if (lineasTipoMuestra.Count() == 1 && lineasTipoMuestra.First().IdTipoMuestra == 0)
                lineasTipoMuestra.Clear();
        }

        private void NuevoTipoMuestra()
        {
            NuevoTipoMuestra nt = new NuevoTipoMuestra();
            nt.Owner = Window.GetWindow(this);
            nt.ShowDialog();
            if (nt.DialogResult ?? false)
            {
                TiposMuestra = TiposMuestra.Insert(nt.TipoMuestra);
                Array.Sort(TiposMuestra);
                panelTipoMuestra["IdTipoMuestra"].InnerValues = TiposMuestra;
                ((DataGridComboBoxColumn)gridTipoMuestra["Tipo de muestra"]).ItemsSource = TiposMuestra;
            }
        }

        private TipoMuestra[] RecuperarTipoMuestra()
        {
            return PersistenceManager.SelectAll<TipoMuestra>().OrderBy(t => t.Nombre).ToArray();
        }

        private void DeleteTipoMuestra(object sender)
        {
            ITipoMuestra lineaBorrada = ((FrameworkElement)sender).DataContext as ITipoMuestra;

            if (CanDeleteTipoMuestra(lineaBorrada.IdTipoMuestra))
            {
                lineasTipoMuestra.Remove(lineaBorrada);
                ActualizarComboParametros(lineasTipoMuestra.Select(l => l.IdTipoMuestra).ToArray());
            }
            else
            {
                MessageBox.Show("Imposible eliminar un tipo de muestra que contiene parámetros");
            }
        }

        private void AddTipoMuestra()
        {
            if (panelTipoMuestra.GetValidatedInnerValue<ITipoMuestra>() != default(ITipoMuestra))
            {
                ITipoMuestra tipoMuestraAdd = panelTipoMuestra.InnerValue.Clone(typeof(ITipoMuestra)) as ITipoMuestra;
                if (!lineasTipoMuestra.Any(l => l.IdTipoMuestra == tipoMuestraAdd.IdTipoMuestra))
                {
                    lineasTipoMuestra.Add(tipoMuestraAdd);
                    ActualizarComboParametros(lineasTipoMuestra.Select(l => l.IdTipoMuestra).ToArray());
                }

                panelTipoMuestra.InnerValue = new ITipoMuestra();
            }
            else
            {
                MessageBox.Show("Datos erróneos. Por favor, revisa la información");
            }
        }

    }
}
