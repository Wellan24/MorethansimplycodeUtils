using Cartif.Util;
using Cartif.XamlResources;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using LAE.Modelo;
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
using System.Collections.Specialized;
using Cartif.Extensions;
using Cartif.Expectation;
using GUI.TreeListView.Tabs;

namespace GUI.Controls
{
    /// <summary>
    /// Lógica de interacción para ControlMuestraRecepAgua.xaml
    /// </summary>
    public partial class ControlMuestraRecepAgua : UserControl
    {
        ObservableCollection<int> listaAlicuotas = new ObservableCollection<int>();

        private int id = 0;
        public int Id
        {
            get
            {
                id++;
                listaAlicuotas.Add(id);
                return id;
            }
        }


        public Action<ControlMuestraRecepAgua> DisabledControl { get; set; }
        public Action<ControlMuestraRecepAgua> EnabledControl { get; set; }

        private MuestraRecepcionAgua muestra;
        public MuestraRecepcionAgua Muestra
        {
            get { return muestra; }
            set
            {
                muestra = value;
                //GenerarBotonBorrado();
                GenerarDatosMuestra();
                GenerarAddAlilcuota();
                GenerarAddParametros();
                ComprobacionTieneTomaMuestra();

            }
        }

        public ControlMuestraRecepAgua()
        {
            InitializeComponent();
        }

        private void GenerarDatosMuestra()
        {
            panelMuestra.Build(Muestra,
                new TypePanelSettings<MuestraRecepcionAgua>
                {
                    Fields = new FieldSettings
                    {
                        ["CodigoToma"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["GetCodigoLae"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetEnabled(false)
                            .SetLabel("Código LAE"),
                        ["Descripcion"] = PropertyControlSettingsEnum.TextBoxDefault,
                    },
                    IsUpdating = true
                }); 
        }

        private void GenerarAddAlilcuota()
        {

            panelAlicuotas.Build(new AlicuotaRecepcionAgua(),
                new TypePanelSettings<AlicuotaRecepcionAgua>
                {
                    ColumnWidths = new int[] { 6, 6, 6, 2, 1, 1, 1 },
                    Fields = new FieldSettings
                    {
                        ["NumeroAlicuotas"] = PropertyControlSettingsEnum.TextBoxDefaultNoEmpty
                            .SetLabel("* Nº ALÍCUOTAS")
                            .SetColumnSpan(1),
                        ["RecipienteVidrio"] = new PropertyControlSettings
                        {
                            InnerValues = new ComboBoxItem<Boolean>[] {
                                ComboBoxItem<Boolean>.Create("Vidrio", true),
                                ComboBoxItem<Boolean>.Create("PE", false)
                            },
                            Type = typeof(PropertyControlComboBox),
                            Label = "Tipo Recipiente",
                        }
                        .SetColumnSpan(1),
                        ["Cantidad"] = PropertyControlSettingsEnum.TextBoxDefaultNoEmpty
                            .SetColumnSpan(1),
                        ["IdUdsCantidad"] = PropertyControlSettingsEnum.ComboBoxDefault
                                        .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Volumen"))
                                        .SetDisplayMemberPath("Abreviatura")
                                        .SetLabel(""),
                        ["BotonAdd"] = new PropertyControlSettings
                        {
                            Type = typeof(PropertyControlButton),
                            DesignPath = CSVPath.ArrowDownIcon,
                            Click = (sender, e) => { AddAlicuota(); },
                        },
                        ["BotonUpdate"] = new PropertyControlSettings
                        {
                            Type = typeof(PropertyControlButton),
                            DesignPath = CSVPath.RefreshIcon,
                            Click = (sender, e) => { UpdateAlicuota(); },
                        },
                        ["BotonDelete"] = new PropertyControlSettings
                        {
                            Type = typeof(PropertyControlButton),
                            DesignPath = CSVPath.RemoveIcon,
                            Click = (sender, e) => { DeleteAlicuota(); },
                        },

                    },
                    PanelValidation = Expectation<AlicuotaRecepcionAgua>
                        .ShouldBe().AddCriteria(a => a.NumeroAlicuotas > 0)
                });




            if (Muestra.Alicuotas.Count() == 1 && Muestra.Alicuotas.First().Id == 0)
                Muestra.Alicuotas.Clear();


        }

        private void AddAlicuota()
        {
            if (panelAlicuotas.GetValidatedInnerValue<AlicuotaRecepcionAgua>() != default(AlicuotaRecepcionAgua))
            {
                AlicuotaRecepcionAgua elementAdd = panelAlicuotas.InnerValue.Clone(typeof(AlicuotaRecepcionAgua)) as AlicuotaRecepcionAgua;
                treeAlicuotas.AddAlicuota(elementAdd);
            }
            else
                MessageBox.Show("Datos erróneos. Por favor, revisa la información");
        }

        private void DeleteAlicuota()
        {
            treeAlicuotas.RemoveAlicuota();
        }

        private void UpdateAlicuota()
        {
            if (panelAlicuotas.GetValidatedInnerValue<AlicuotaRecepcionAgua>() != default(AlicuotaRecepcionAgua))
            {
                AlicuotaRecepcionAgua update = panelAlicuotas.InnerValue as AlicuotaRecepcionAgua;
                treeAlicuotas.UpdateAlicuota(update);
            }
            else
                MessageBox.Show("Datos erróneos. Por favor, revisa la información");

        }

        private void GenerarAddParametros()
        {
            if (Muestra.Parametros.Count() == 1 && Muestra.Parametros.First().Id == 0)
                Muestra.Parametros.Clear();
        }

        private void ComprobacionTieneTomaMuestra()
        {
            if (Muestra.TieneTomaMuestra)
            {
                panelAlicuotas["BotonAdd"].Visibility = Visibility.Collapsed;
                panelAlicuotas["BotonDelete"].Visibility = Visibility.Collapsed;
                panelAlicuotas["RecipienteVidrio"].Visibility = Visibility.Collapsed;

                panelMuestra["CodigoToma"].IsEnabled = false;
            }
        }

        public bool Validar()
        {
            /* no valido los paneles que sean para añadir datos a un grid */
            bool validacion = panelMuestra.GetValidatedInnerValue<MuestraRecepcionAgua>() != default(MuestraRecepcionAgua);
            if (!validacion)
                return false;

            /* valido que los parametros este incluidos en una alicuota */
            AlicuotaRecepcionAguaModel modelo = treeAlicuotas.tree.Model as AlicuotaRecepcionAguaModel;
            foreach (Item item in modelo.Root.Items)
            {
                if (!(item is AlicuotaItem))
                    return false;
            }
            return true;
        }

        public void SetEnabledControl(bool enabled)
        {
            panelMuestra.IsEnabled = enabled;
            panelAlicuotas.IsEnabled = enabled;
        }
    }
}
