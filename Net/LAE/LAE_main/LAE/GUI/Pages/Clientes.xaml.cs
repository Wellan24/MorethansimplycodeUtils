using Cartif.Extensions;
using Cartif.Util;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using LAE.Clases;
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
    /// Lógica de interacción para Clientes.xaml
    /// </summary>
    public partial class Clientes : UserControl
    {
        private bool cargar = false;
        public ObservableCollection<Cliente> ListaClientes;

        public Clientes()
        {
            InitializeComponent();
            //CargarClientes();
        }

        private void CargarClientes()
        {
            ListaClientes = new ObservableCollection<Cliente> { new Cliente() };

            panelClientes.Build<Cliente>(new Cliente() { Pais = "España" },
                new TypePanelSettings<Cliente>
                {
                    Fields = new FieldSettings
                    {
                        ["Nombre"] = PropertyControlSettingsEnum.TextBoxDefaultNoEmpty
                                    .SetLabel("* Nombre"),
                        ["Direccion"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["CodigoPostal"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["Localidad"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["Provincia"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["Pais"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["Telefono"] = PropertyControlSettingsEnum.TextBoxDefault,
                        //["Fax"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["Email"] = PropertyControlSettingsEnum.TextBoxIsValidEmailOrEmpty,
                        //["Web"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["Cif"] = PropertyControlSettingsEnum.TextBoxDefault,

                    },
                    PanelValidation = Expectation<Cliente>
                        .Should().AddTest(c => Util.ValorUnicoOVacio<Cliente>("Cif", c))
                        .FollowingShould().AddTest(c => Util.ValorUnicoOVacio<Cliente>("Email", c))
                });

            gridClientes.Build(ListaClientes,
                new TypeGridSettings
                {
                    Columns = new ColumnGridSettings
                    {
                        ["Nombre"] = TypeGridColumnSettingsEnum.DefaultColum,
                        ["Direccion"] = TypeGridColumnSettingsEnum.DefaultColum,
                        ["Telefono"] = TypeGridColumnSettingsEnum.DefaultColum,
                        //["Fax"] = TypeGridColumnSettingsEnum.DefaultColum,
                        ["Email"] = TypeGridColumnSettingsEnum.DefaultColum,
                        //["Web"] = TypeGridColumnSettingsEnum.DefaultColum,
                        ["Cif"] = TypeGridColumnSettingsEnum.DefaultColum,
                    }
                });

            ListaClientes = new ObservableCollection<Cliente>(PersistenceManager.SelectAll<Cliente>().OrderBy(c => c.Nombre));
            gridClientes.FillDataGrid(ListaClientes);
        }

        private void ButtonGuardarCliente_Click(object sender, RoutedEventArgs e)
        {
            FormBasicFunctions.GuardarDatos<Cliente>(panelClientes, gridClientes, ListaClientes, "Cliente");
            CambiarFoco();
        }

        private void ButtonNuevoCliente_Click(object sender, RoutedEventArgs e)
        {
            Nuevo();
            CambiarFoco();
        }

        private void ButtonBorrarCliente_Click(object sender, RoutedEventArgs e)
        {
            if (FormBasicFunctions.BorrarDato<Cliente>(panelClientes, gridClientes, ListaClientes, "Cliente"))
                Nuevo();
            CambiarFoco();
        }

        private void Nuevo()
        {
            gridClientes.dataGrid.SelectedIndex = -1;
            panelClientes.InnerValue = new Cliente() { Pais = "España" };
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (cargar)
            {
                panelClientes.ClearGrid();
                CargarClientes();
            }
            else
                cargar = true;
        }

        private void CambiarFoco()
        {
            if ((panelClientes["NombreParametro"] as PropertyControlTextBox) != null)
                Keyboard.Focus(((PropertyControlTextBox)panelClientes["NombreParametro"]).innerContent);
        }
    }
}
