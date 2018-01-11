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
    /// Lógica de interacción para Contactos.xaml
    /// </summary>
    public partial class Contactos : UserControl
    {
        private bool cargar = false;
        public List<Contacto> ListaContactos;

        public Contactos()
        {
            InitializeComponent();
            
        }

        private void CargarContactos()
        {

            Cliente[] clientes = PersistenceManager.SelectAll<Cliente>().OrderBy(c => c.Nombre).ToArray();
            Tecnico[] tecnicos = PersistenceManager.SelectAll<Tecnico>().OrderBy(c => c.Id).ToArray();

            panelContactos.Build(new Contacto(),
                new TypePanelSettings<Contacto>
                {
                    Fields = new FieldSettings
                    {
                        ["Nombre"] = PropertyControlSettingsEnum.TextBoxDefaultNoEmpty
                                .SetLabel("* Nombre"),
                        ["Apellidos"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["Telefono"] = PropertyControlSettingsEnum.TextBoxDefault
                                .SetLabel("Teléfono/s"),
                        ["Email"] = PropertyControlSettingsEnum.TextBoxIsValidEmail
                                .SetLabel("* Email"),
                        ["IdCliente"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                .SetInnerValues(clientes)
                                .SetLabel("* Empresa"),
                        ["IdTecnico"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                .SetInnerValues(tecnicos)
                                .SetLabel("* Técnico")
                    },
                    PanelValidation = Expectation<Contacto>
                        .ShouldBe().AddCriteria(c => Util.ValorUnico("Email", c))
                });


            gridContactos.Build<Contacto>(new TypeGridSettings()
            {
                Columns = new ColumnGridSettings
                {
                    ["Nombre"] = TypeGridColumnSettingsEnum.DefaultColum,
                    ["Apellidos"] = TypeGridColumnSettingsEnum.DefaultColum,
                    ["Telefono"] = TypeGridColumnSettingsEnum.DefaultColum
                            .SetLabel("Teléfono/s"),
                    ["Email"] = TypeGridColumnSettingsEnum.DefaultColum,
                    ["IdCliente"] = new TypeGridColumnSettings
                    {
                        Label = "Empresa",
                        ColumnCombo = new TypeGCComboSettings
                        {
                            InnerValues = clientes,
                            Path = "Id"
                        }
                    },
                    ["IdTecnico"] = new TypeGridColumnSettings
                    {
                        Label = "Técnico",
                        ColumnCombo = new TypeGCComboSettings
                        {
                            InnerValues = tecnicos,
                            Path = "Id"
                        }
                    },
                },

            });

            ListaContactos = PersistenceManager.SelectAll<Contacto>().OrderBy(c => c.Nombre).ThenBy(c => c.Apellidos).ToList();
            gridContactos.FillDataGrid(ListaContactos);
        }

        private void ButtonGuardarCliente_Click(object sender, RoutedEventArgs e)
        {
            FormBasicFunctions.GuardarDatos<Contacto>(panelContactos, gridContactos, "Contacto");
            CambiarFoco();
        }

        private void ButtonNuevoCliente_Click(object sender, RoutedEventArgs e)
        {
            Nuevo();
            CambiarFoco();
        }

        private void ButtonBorrarCliente_Click(object sender, RoutedEventArgs e)
        {
            if (FormBasicFunctions.BorrarDato<Contacto>(panelContactos, gridContactos, "Contacto"))
                Nuevo();
            CambiarFoco();
        }

        private void Nuevo()
        {
            gridContactos.DataGrid.SelectedIndex = -1;
            panelContactos.InnerValue = new Contacto();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (cargar)
            {
                panelContactos.ClearGrid();
                CargarContactos();
            }
            else
                cargar = true;
        }

        private void CambiarFoco()
        {
            if ((panelContactos["NombreParametro"] as PropertyControlTextBox) != null)
                Keyboard.Focus(((PropertyControlTextBox)panelContactos["NombreParametro"]).InnerContent);
        }
    }
}
