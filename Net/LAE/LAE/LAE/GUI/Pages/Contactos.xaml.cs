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

namespace GUI.Pages
{
    /// <summary>
    /// Lógica de interacción para Contactos.xaml
    /// </summary>
    public partial class Contactos : UserControl
    {
        public ObservableCollection<Contacto> ListaContactos;

        public Contactos()
        {
            InitializeComponent();
            CargarContactos();
        }

        private void CargarContactos()
        {
            ListaContactos = new ObservableCollection<Contacto>(PersistenceManager<Contacto>.SelectAll().OrderBy(c => c.Id).ToArray());

            Cliente[] clientes = PersistenceManager<Cliente>.SelectAll().OrderBy(c => c.Id).ToArray();
            Tecnico[] tecnicos = PersistenceManager<Tecnico>.SelectAll().OrderBy(c => c.Id).ToArray();

            panelContactos.Build<Contacto>(new Contacto(),
                new TypePanelSettings<Contacto>
                {
                    Fields = new FieldSettings
                    {
                        ["Id"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["Nombre"] = PropertyControlSettingsEnum.TextBoxDefaultNoEmpty,
                        ["Apellidos"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["Telefono"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["Email"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["IdCliente"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                .SetInnerValues(clientes)
                                .SetLabel("Cliente"),
                        ["IdTecnico"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                .SetInnerValues(tecnicos)
                                .SetLabel("Técnico")
                    }
                });

            
            gridContactos.Build(ListaContactos, new TypeGridSettings()
            {
                Columns = new ColumnGridSettings
                {
                    ["Id"] = TypeGridColumnSettingsEnum.DefaultColum,
                    ["Nombre"] = TypeGridColumnSettingsEnum.DefaultColum,
                    ["Apellidos"] = TypeGridColumnSettingsEnum.DefaultColum,
                    ["Telefono"] = TypeGridColumnSettingsEnum.DefaultColum,
                    ["Email"] = TypeGridColumnSettingsEnum.DefaultColum,
                    ["IdCliente"] = new TypeGridColumnSettings
                    {
                        Label = "Cliente",
                        ColumnCombo = new TypeGCComboSettings
                        {
                            InnerValues = clientes,
                            Path="Id"
                        }
                    },
                    ["IdTecnico"] = new TypeGridColumnSettings
                    {
                        Label = "Técnico",
                        ColumnCombo = new TypeGCComboSettings
                        {
                            InnerValues = tecnicos,
                            Path="Id"
                        }
                    },
                },

            });

            gridContactos.dataGrid.SelectedIndex = 0;
        }

        private void ButtonGuardarCliente_Click(object sender, RoutedEventArgs e)
        {
            FormBasicFunctions.GuardarDatos<Contacto>(panelContactos, gridContactos, ListaContactos, "Contacto");
        }

        private void ButtonNuevoCliente_Click(object sender, RoutedEventArgs e)
        {
            panelContactos.InnerValue = new Contacto();
        }

        private void ButtonBorrarCliente_Click(object sender, RoutedEventArgs e)
        {
            FormBasicFunctions.BorrarDato<Contacto>(panelContactos, gridContactos, ListaContactos, "Contacto");
        }
    }
}
