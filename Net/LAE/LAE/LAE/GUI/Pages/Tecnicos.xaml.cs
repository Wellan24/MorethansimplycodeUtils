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
    /// Lógica de interacción para Tecnicos.xaml
    /// </summary>
    public partial class Tecnicos : UserControl
    {
        public ObservableCollection<Tecnico> ListaTecnicos;

        public Tecnicos()
        {
            InitializeComponent();
            CargarTecnicos();
        }

        private void CargarTecnicos()
        {
            ListaTecnicos = new ObservableCollection<Tecnico>( PersistenceManager<Tecnico>.SelectAll().OrderBy(c => c.Id));

            panelTecnicos.Build<Tecnico>(new Tecnico());

            gridTecnicos.Build(ListaTecnicos);
            gridTecnicos.dataGrid.SelectedIndex = 0;
        }

        private void ButtonGuardarCliente_Click(object sender, RoutedEventArgs e)
        {
            FormBasicFunctions.GuardarDatos<Tecnico>(panelTecnicos, gridTecnicos, ListaTecnicos, "Técnico");
        }

        private void ButtonNuevoCliente_Click(object sender, RoutedEventArgs e)
        {
            panelTecnicos.InnerValue = new Tecnico();
        }

        private void ButtonBorrarCliente_Click(object sender, RoutedEventArgs e)
        {
            FormBasicFunctions.BorrarDato<Tecnico>(panelTecnicos, gridTecnicos, ListaTecnicos, "Técnico");
        }
    }
}
