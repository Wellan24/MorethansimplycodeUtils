using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
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

namespace GUI.Pages
{
    /// <summary>
    /// Lógica de interacción para Parametros.xaml
    /// </summary>
    public partial class Parametros : UserControl
    {
        private bool cargar = false;
        public ObservableCollection<Parametro> ListaParametros;

        public Parametros()
        {
            InitializeComponent();
        }

        private void CargarParametros()
        {
            ListaParametros = new ObservableCollection<Parametro> { new Parametro() };

            TipoMuestra[] tiposMuestra = PersistenceManager.SelectAll<TipoMuestra>().OrderBy(t => t.Nombre).ToArray();

            panelParametros.Build(new Parametro(),
                new TypePanelSettings<Parametro>
                {
                    Fields = new FieldSettings
                    {
                        ["NombreParametro"] = PropertyControlSettingsEnum.TextBoxDefaultNoEmpty
                                    .SetLabel("* Nombre"),
                        ["MetodoParametro"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetLabel("Método"),
                        ["Norma"] = PropertyControlSettingsEnum.TextBoxDefault,

                        ["Conservacion"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetLabel("Conservación"),
                        ["IdTipoMuestra"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                .SetInnerValues(tiposMuestra)
                                .SetLabel("* Tipo de muestra"),
                    },
                });

            gridParametros.Build(ListaParametros,
                new TypeGridSettings
                {
                    Columns = new ColumnGridSettings
                    {
                        ["NombreParametro"] = TypeGridColumnSettingsEnum.DefaultColum,
                        ["MetodoParametro"] = TypeGridColumnSettingsEnum.DefaultColum,
                        ["Norma"] = TypeGridColumnSettingsEnum.DefaultColum,
                        ["Conservacion"] = TypeGridColumnSettingsEnum.DefaultColum,
                        ["IdTipoMuestra"] = new TypeGridColumnSettings
                        {
                            Label = "Tipo de muestra",
                            ColumnCombo = new TypeGCComboSettings
                            {
                                InnerValues = tiposMuestra,
                                Path = "Id"
                            }
                        },
                    }
                });

            //ListaParametros = new ObservableCollection<Parametro>(PersistenceManager<Parametro>.SelectAll().OrderBy(c => c.NombreParametro));
            ListaParametros = new ObservableCollection<Parametro>(FactoriaParametros.ParametrosOrdenados());
            gridParametros.FillDataGrid(ListaParametros);
        }

        private void ButtonGuardarParametro_Click(object sender, RoutedEventArgs e)
        {
            FormBasicFunctions.GuardarDatos(panelParametros, gridParametros, ListaParametros, "Parámetro");
            CambiarFoco();
        }

        private void ButtonNuevoParametro_Click(object sender, RoutedEventArgs e)
        {
            Nuevo();
            CambiarFoco();
        }

        private void ButtonBorrarParametro_Click(object sender, RoutedEventArgs e)
        {
            if (FormBasicFunctions.BorrarDato<Parametro>(panelParametros, gridParametros, ListaParametros, "Parámetro"))
                Nuevo();
            CambiarFoco();
        }

        private void Nuevo()
        {
            gridParametros.DataGrid.SelectedIndex = -1;
            panelParametros.InnerValue = new Parametro();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (cargar)
            {
                panelParametros.ClearGrid();
                CargarParametros();
            }
            else
                cargar = true;
        }

        private void CambiarFoco()
        {
            if ((panelParametros["NombreParametro"] as PropertyControlTextBox) !=null)
                Keyboard.Focus(((PropertyControlTextBox)panelParametros["NombreParametro"]).InnerContent);
        }
    }
}
