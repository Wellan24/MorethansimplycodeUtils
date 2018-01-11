using Cartif.Extensions;
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
    /// Lógica de interacción para Clientes.xaml
    /// </summary>
    public partial class Clientes : UserControl
    {
        public ObservableCollection<Cliente> ListaClientes;

        public Clientes()
        {
            InitializeComponent();
            CargarClientes();
        }

        private void CargarClientes()
        {
            ListaClientes = new ObservableCollection<Cliente>(PersistenceManager<Cliente>.SelectAll().OrderBy(c => c.Id));

            panelClientes.Build<Cliente>(new Cliente());

            gridClientes.Build(ListaClientes);
            gridClientes.dataGrid.SelectedIndex = 0;
        }

        private void ButtonGuardarCliente_Click(object sender, RoutedEventArgs e)
        {
            FormBasicFunctions.GuardarDatos<Cliente>(panelClientes, gridClientes, ListaClientes, "Cliente");
            //if (panelClientes.GetValidatedInnerValue<Cliente>() != default(Cliente))
            //{
            //    Cliente clienteSeleccionado = panelClientes.InnerValue as Cliente;
            //    if (clienteSeleccionado.Id != 0)
            //    {
            //        /* update cliente */
            //        clienteSeleccionado.Update();

            //        /* update grid */
            //        int indice = ListaClientes.IndexOf(clienteSeleccionado);
            //        ListaClientes[indice] = clienteSeleccionado;
            //        gridClientes.FillDataGrid(ListaClientes);

            //        gridClientes.dataGrid.SelectedIndex = indice;
            //    }
            //    else {
            //        /* insert cliente */
            //        int idCliente = clienteSeleccionado.Insert();
            //        clienteSeleccionado.Id = idCliente;
            //        /* update grid */
            //        ListaClientes.Add(clienteSeleccionado);

            //        gridClientes.dataGrid.SelectedIndex = gridClientes.dataGrid.Items.Count - 1;
            //    }

            //    MessageBox.Show("Cliente actualizado");
            //}
            //else
            //{
            //    MessageBox.Show("Algún dato es erroneo");
            //}
        }



        private void ButtonNuevoCliente_Click(object sender, RoutedEventArgs e)
        {
            panelClientes.InnerValue = new Cliente();
        }

        private void ButtonBorrarCliente_Click(object sender, RoutedEventArgs e)
        {
            FormBasicFunctions.BorrarDato<Cliente>(panelClientes, gridClientes, ListaClientes, "Cliente");
            //Cliente clienteSeleccionado = panelClientes.InnerValue as Cliente;
            //if (clienteSeleccionado != null && clienteSeleccionado?.Id != 0)
            //{
            //    MessageBoxResult messageBoxResult = MessageBox.Show("¿Estas seguro de borrar el cliente?", "Borrar cliente", MessageBoxButton.YesNo);
            //    if (messageBoxResult == MessageBoxResult.Yes)
            //    {
            //        clienteSeleccionado.Delete();
            //        ListaClientes.Remove(clienteSeleccionado);
            //        gridClientes.dataGrid.SelectedIndex = 0;
            //    }
            //}
        }
    }
}
