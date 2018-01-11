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

        public Peticiones()
        {
            InitializeComponent();
        }

        public Peticiones(Cliente[] clientes, Tecnico[] tecnicos, Peticion peticion)
        {
            InitializeComponent();
            UCPeticion.Clientes = clientes;
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
                MessageBox.Show("Datos guardados con exito");
                DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Algún dato es erroneo");
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
            List<TipoMuestraPeticion> lineas = PersistenceManager<TipoMuestraPeticion>.SelectByProperty("IdPeticion", pet.Id).ToList();
            
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
                    //tmp.Update();

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
            List<LineasPeticion> lineas = PersistenceManager<LineasPeticion>.SelectByProperty("IdPeticion", pet.Id).ToList();

            foreach (ILineasParametros item in UCPeticion.lineasParametros)
            {
                LineasPeticion tmp = new LineasPeticion();
                tmp.Cantidad = item.Cantidad;
                tmp.Metodo = item.Metodo;
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

    }
}
