using System;
using System.Collections.Generic;
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
using Cartif.Extensions;
using LAE.Comun.Persistence;
using LAE.Modelo;
using LAE.Comun.Modelo;

namespace GUI.Pages
{
    /// <summary>
    /// Lógica de interacción para Ofertas.xaml
    /// </summary>
    public partial class Ofertas : UserControl
    {
        private bool cargar = false;

        private Cliente[] Clientes;
        private Contacto[] Contactos;
        private Tecnico[] Tecnicos;
        private Oferta o;

        public Ofertas()
        {
            InitializeComponent();
            UCListaOfertas.VerDetallesClick += (s, e) =>
            {
                o = UCListaOfertas.SelectedOferta;
                if (o?.Id != 0)
                {
                    UCDetalleOferta.Oferta = o;
                    Mostrar(UCDetalleOferta);
                }
                else
                {
                    MessageBox.Show("Selecciona una oferta para ver sus detalles");
                }

            };
            UCDetalleOferta.BackButtonClick += (s, e) =>
            {
                Mostrar(UCListaOfertas);
            };
        }

        public void Mostrar(Object paraMostrar)
        {
            UserControl uc = paraMostrar as UserControl;
            if (uc != null)
            {
                root.Children.OfType<UserControl>().Where(u => u.Visibility == Visibility.Visible).ForEach(u => u.Visibility = Visibility.Collapsed);
                uc.Visibility = Visibility.Visible;
            }
        }

        private void pageOfertas_Loaded(object sender, RoutedEventArgs e)
        {
            if (cargar)
            {
                CargarDatos();
                Mostrar(UCListaOfertas);
            }
            else
                cargar = true;
        }

        private void CargarDatos()
        {
            Clientes = PersistenceManager.SelectAll<Cliente>()
                .OrderByDescending(c => c.Nombre).ToArray();
            Contactos = PersistenceManager.SelectAll<Contacto>()
                .OrderByDescending(c => c.Nombre).ThenBy(c => c.Apellidos).ToArray();
            Tecnicos = PersistenceManager.SelectAll<Tecnico>()
                .OrderByDescending(c => c.Nombre).ThenBy(c => c.PrimerApellido).ThenBy(c => c.SegundoApellido).ToArray();

            UCListaOfertas.CargarDatosIniciales(Clientes, Contactos, Tecnicos);
            UCDetalleOferta.CargarDatosIniciales(Clientes, Contactos, Tecnicos);
        }
    }
}
