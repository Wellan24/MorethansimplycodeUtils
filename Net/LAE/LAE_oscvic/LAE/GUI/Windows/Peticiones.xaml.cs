using LAE.Clases;
using LAE.Modelo;
using MahApps.Metro.Controls;
using Persistence;
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
using System.Windows.Shapes;

namespace GUI.Windows
{
    /// <summary>
    /// Lógica de interacción para Peticiones.xaml
    /// </summary>
    public partial class Peticiones : MetroWindow
    {

        public static readonly DependencyProperty IconTitleProperty =
            DependencyProperty.Register("IconTitle", typeof(ImageSource), typeof(Peticiones), new PropertyMetadata(null));
        public ImageSource IconTitle
        {
            get { return (ImageSource)GetValue(IconTitleProperty); }
            set { SetValue(IconTitleProperty, value); }
        }

        public Peticiones()
        {
            InitializeComponent();
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE;component/images/cabecera.png", UriKind.Absolute));
        }

        public Peticiones(Cliente[] clientes, Tecnico[] tecnicos, Peticion peticion)
        {
            InitializeComponent();
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE;component/images/cabecera.png", UriKind.Absolute));
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

            CargarDatos(clientes, tecnicos, peticion);
        }

        public Peticiones(Cliente[] clientes, Tecnico[] tecnicos, Peticion peticion, Oferta o)
        {
            InitializeComponent();
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE;component/images/cabecera.png", UriKind.Absolute));

            peticion.IdCliente = o.IdCliente;
            peticion.IdContacto = o.IdContacto;
            peticion.IdTecnico = o.IdTecnico;
            peticion.Fecha = DateTime.Now;

            CargarDatos(clientes, tecnicos, peticion);
        }

        private void CargarDatos(Cliente[] clientes, Tecnico[] tecnicos, Peticion peticion)
        {
            UCPeticion.Tecnicos = tecnicos;
            UCPeticion.Peticion = peticion;
        }

        private void bGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (UCPeticion.ValidarPeticion())
            {
                Peticion pet = UCPeticion.Peticion;
                GuardarPeticion(pet);
                GuardarTipoMuestra(pet);
                GuardarParametros(pet);
                MessageBox.Show("Datos guardados con éxito");
                DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Datos erróneos. Por favor, revisa la información");
            }
        }

        private void GuardarPeticion(Peticion pet)
        {
            if (pet.Id == 0)
                pet.Insert();
            else
                pet.Update();
        }

        private void GuardarTipoMuestra(Peticion pet)
        {
            List<TipoMuestraPeticion> lineas = PersistenceManager.SelectByProperty<TipoMuestraPeticion>("IdPeticion", pet.Id).ToList();

            foreach (ITipoMuestra item in UCPeticion.lineasTipoMuestra)
            {
                TipoMuestraPeticion tmp = new TipoMuestraPeticion();
                tmp.IdTipoMuestra = item.IdTipoMuestra;

                if (item.Id == 0)
                {
                    /* inserto nuevos */
                    tmp.IdPeticion = pet.Id;
                    tmp.Insert();
                }
                else
                {
                    /* actualizo existentes */
                    tmp.Id = item.Id;
                    tmp.IdPeticion = item.IdRelacion;
                    tmp.Update();

                    lineas.Remove(tmp);
                }
            }
            foreach (TipoMuestraPeticion item in lineas)
            {
                /* elimino borrados */
                item.Delete();
            }
        }

        private void GuardarParametros(Peticion pet)
        {
            List<LineasPeticion> lineas = PersistenceManager.SelectByProperty<LineasPeticion>("IdPeticion", pet.Id).ToList();

            foreach (ILineasParametros item in UCPeticion.lineasParametros)
            {
                LineasPeticion tmp = new LineasPeticion();
                tmp.Cantidad = item.Cantidad;
                tmp.IdParametro = item.IdParametro;

                if (item.Id == 0)
                {
                    /* inserto nuevos */
                    tmp.IdPeticion = pet.Id;
                    tmp.Insert();
                }
                else
                {
                    /* actualizo existentes */
                    tmp.IdPeticion = pet.Id;
                    tmp.Id = item.Id;
                    tmp.Update();

                    lineas.Remove(tmp);
                }
            }
            foreach (LineasPeticion item in lineas)
            {
                /* elimino borrados */
                item.Delete();
            }
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void ComboCargar_Click(object sender, RoutedEventArgs e)
        {
            ComboCargarRevision combo = new ComboCargarRevision();
            combo.Lista = Util.ComboBoxCodigoOfertas();
            combo.Owner = Window.GetWindow(this);
            combo.ShowDialog();

            if (combo.DialogResult ?? false)
            {
                RevisionOferta r = PersistenceManager.SelectByID<RevisionOferta>(combo.idSeleccionado);
                Oferta o = Util.GetOfertaFromRevision(combo.idSeleccionado);

                Peticion pet = Util.GenerarPeticionFromRevision(r, o);
                UCPeticion.CargarNuevaPeticion(pet);
                UCPeticion.CargarTipoMuestra(Util.GetTiposMuestraFromRevision(combo.idSeleccionado));
                UCPeticion.CargarParametro(Util.GetParametrosFromRevision(combo.idSeleccionado));

                MessageBox.Show("Datos cargados con éxito");
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Util.VisualizeWindow(this);
        }
    }
}
