using Cartif.Expectation;
using Cartif.Util;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using LAE.Comun.Clases;
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
    /// Lógica de interacción para Tecnicos.xaml
    /// </summary>
    public partial class Tecnicos : UserControl
    {
        private bool cargar = false;
        public List<Tecnico> ListaTecnicos;

        public Tecnicos()
        {
            InitializeComponent();
            GenerarGridTecnicos();
        }

        private void GenerarGridTecnicos()
        {
            gridTecnicos.Build<Tecnico>(
                new TypeGridSettings
                {
                    Columns = new ColumnGridSettings
                    {
                        ["Dni"] = TypeGridColumnSettingsEnum.DefaultColum,
                        ["Nombre"] = TypeGridColumnSettingsEnum.DefaultColum,
                        ["PrimerApellido"] = TypeGridColumnSettingsEnum.DefaultColum,
                        ["SegundoApellido"] = TypeGridColumnSettingsEnum.DefaultColum,
                        ["Director"] = new TypeGridColumnSettings
                        {
                            ColumnCombo = new TypeGCComboSettings
                            {
                                InnerValues = new ComboBoxItem<Boolean>[] {
                                    ComboBoxItem<Boolean>.Create("No", false),
                                    ComboBoxItem<Boolean>.Create("Si", true)
                                },
                                Path = "Id"
                            }
                        }
                        .SetLabel("Director/a"),
                    }
                });
        }

        private void CargarTecnicos()
        {
            panelTecnicos.Build(new Tecnico(),
                new TypePanelSettings<Tecnico>
                {
                    Fields = new FieldSettings
                    {
                        ["Dni"] = PropertyControlSettingsEnum.TextBoxDefaultNoEmpty
                                    .SetLabel("* Dni"),
                        ["Nombre"] = PropertyControlSettingsEnum.TextBoxDefaultNoEmpty
                                    .SetLabel("* Nombre"),
                        ["PrimerApellido"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["SegundoApellido"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["Director"] = new PropertyControlSettings
                        {
                            InnerValues = new ComboBoxItem<Boolean>[] {
                                ComboBoxItem<Boolean>.Create("Si", true),
                                ComboBoxItem<Boolean>.Create("No", false)
                            },
                            Type = typeof(PropertyControlComboBox)
                        }
                        .SetLabel("Director/a"),
                    },
                    PanelValidation = Expectation<Tecnico>.ShouldBe().AddCriteria(t => Util.ValorUnico<Tecnico>("Dni", t))
                });

            
            ListaTecnicos =PersistenceManager.SelectAll<Tecnico>().OrderBy(c => c.Nombre).ThenBy(c => c.PrimerApellido).ThenBy(c => c.SegundoApellido).ToList();
            gridTecnicos.FillDataGrid(ListaTecnicos);
        }

        private void ButtonGuardarCliente_Click(object sender, RoutedEventArgs e)
        {
            FormBasicFunctions.GuardarDatos<Tecnico>(panelTecnicos, gridTecnicos, "Técnico");
            CambiarFoco();
        }

        private void ButtonNuevoCliente_Click(object sender, RoutedEventArgs e)
        {
            Nuevo();
            CambiarFoco();
        }

        private void ButtonBorrarCliente_Click(object sender, RoutedEventArgs e)
        {
            if (FormBasicFunctions.BorrarDato<Tecnico>(panelTecnicos, gridTecnicos, "Técnico"))
                Nuevo();
            CambiarFoco();
        }

        private void Nuevo()
        {
            gridTecnicos.DataGrid.SelectedIndex = -1;
            panelTecnicos.InnerValue = new Tecnico();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (cargar)
            {
                panelTecnicos.ClearGrid();
                CargarTecnicos();
            }
            else
                cargar = true;
        }

        private void CambiarFoco()
        {
            if ((panelTecnicos["Dni"] as PropertyControlTextBox) != null)
                Keyboard.Focus(((PropertyControlTextBox)panelTecnicos["Dni"]).InnerContent);
        }
    }
}
