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

namespace GUI.Wizards
{
    /// <summary>
    /// Lógica de interacción para NuevaPeticion.xaml
    /// </summary>
    public partial class NuevaPeticion : MetroWindow
    {
        public NuevaPeticion()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            Cliente[] clientes = RecuperarClientes();
            Tecnico[] tecnicos = RecuperarTecnicos();

            CargarPeticion(clientes, tecnicos);
            CargarOferta(clientes, tecnicos);
            CargarRevision(tecnicos);
        }

        private void CargarPeticion(Cliente[] clientes, Tecnico[] tecnicos)
        {
            UCPeticion.Clientes = clientes;
            UCPeticion.Tecnicos = tecnicos;
            UCPeticion.Peticion = new Peticion() {
                Fecha=DateTime.Now
            };
        }

        private void CargarOferta(Cliente[] clientes, Tecnico[] tecnicos)
        {
            UCOferta.Clientes = clientes;
            UCOferta.Tecnicos = tecnicos;
        }

        private void CargarRevision(Tecnico[] tecnicos)
        {
            UCRevision.Tecnicos = tecnicos;
        }

        private Oferta GenerarDatosOferta()
        {
            Peticion p = UCPeticion.Peticion;
            Oferta o = new Oferta
            {
                AnnoOferta = p.Fecha?.Year ?? DateTime.Now.Year,
                IdCliente = p.IdCliente,
                IdContacto = p.IdContacto,
                IdTecnico = p.IdTecnico
            };

            return o;
        }

        private RevisionOferta GenerarDatosRevision()
        {
            Peticion p = UCPeticion.Peticion;
            Oferta o = UCOferta.Oferta;
            RevisionOferta r = new RevisionOferta
            {
                FechaEmision = p.Fecha,
                Frecuencia = p.Frecuencia,
                IdTecnico = o.IdTecnico,
                LugarMuestra = p.LugarMuestra,
                NumPuntosMuestreo = p.NumPuntosMuestreo,
                PlazoRealizacion = p.PlazoRealizacion,
                RequiereTomaMuestra = p.RequiereTomaMuestra,
                TrabajoPuntual = p.TrabajoPuntual
            };
            return r;
        }

        private void bNext1_Click(object sender, RoutedEventArgs e)
        {
            if ((!rWithPeticion.IsChecked ?? false) || UCPeticion.ValidarPeticion())
            {
                if (UCOferta.Oferta == default(Oferta))
                    if (rWithPeticion.IsChecked ?? false)
                        UCOferta.Oferta = GenerarDatosOferta();
                    else
                        UCOferta.Oferta = new Oferta {
                            AnnoOferta = DateTime.Now.Year
                        };
                Tab2.IsSelected = true;
            }
            else
            {
                MessageBox.Show("Algún dato es erroneo");
            }
        }

        private void bPrev1_Click(object sender, RoutedEventArgs e)
        {
            Tab1.IsSelected = true;
        }

        private void bPrev2_Click(object sender, RoutedEventArgs e)
        {
            Tab2.IsSelected = true;
        }



        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Estas seguro?", "Cancelar", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                DialogResult = false;
                this.Close();
            }
        }

        private Cliente[] RecuperarClientes()
        {
            return PersistenceManager<Cliente>.SelectAll().OrderBy(c => c.Nombre).ToArray();
        }

        private Tecnico[] RecuperarTecnicos()
        {
            return PersistenceManager<Tecnico>.SelectAll().OrderBy(t => t.Nombre).ToArray();
        }

        private void bGuardar1_Click(object sender, RoutedEventArgs e)
        {
            if (UCOferta.ValidarOferta())
            {
                int idOferta = GuardarOferta();
                if (rWithPeticion.IsChecked ?? false)
                    GuardarPeticion(idOferta);

                MessageBox.Show("Datos guardados con exito");
                DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Algún dato es erroneo");
            }
        }

        private void bNext2_Click(object sender, RoutedEventArgs e)
        {
            if (UCOferta.ValidarOferta())
            {
                if (UCRevision.Revision == default(RevisionOferta))
                    if (rWithPeticion.IsChecked ?? false)
                    {
                        UCRevision.Revision = GenerarDatosRevision();
                        UCRevision.CargarTipoMuestra(UCPeticion.lineasTipoMuestra);
                        UCRevision.CargarParametro(UCPeticion.lineasParametros);
                    }
                    else
                        UCRevision.Revision = new RevisionOferta() {
                            FechaEmision = DateTime.Now
                        };
                Tab3.IsSelected = true;
                
            }
            else
            {
                MessageBox.Show("Algún dato es erroneo");
            }
        }

        private void bGuardar2_Click(object sender, RoutedEventArgs e)
        {
            if (UCRevision.ValidarRevision())
            {
                int idOferta = GuardarOferta();

                if (rWithPeticion.IsChecked ?? false)
                    GuardarPeticion(idOferta);

                GuardarRevision(idOferta);

                MessageBox.Show("Datos guardados con exito");
                DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Algún dato es erroneo");
            }
        }

        private int GuardarOferta()
        {
            int idOferta = UCOferta.Oferta.Insert();
            return idOferta;
        }

        private void GuardarPeticion(int idOferta)
        {
            Peticion pet = UCPeticion.Peticion;
            pet.IdOferta = idOferta;
            int idPeticion = pet.Insert();

            foreach (ITipoMuestra item in UCPeticion.lineasTipoMuestra)
            {
                TipoMuestraPeticion tmp = new TipoMuestraPeticion
                {
                    IdTipoMuestra = item.IdTipoMuestra,
                    IdPeticion = idPeticion
                };
                tmp.Insert();
            }

            foreach (ILineasParametros item in UCPeticion.lineasParametros)
            {
                LineasPeticion lp = new LineasPeticion
                {
                    IdParametro = item.IdParametro,
                    Cantidad = item.Cantidad,
                    Metodo = item.Metodo,
                    IdPeticion = idPeticion
                };
                lp.Insert();
            }
        }

        private void GuardarRevision(int idOferta)
        {
            RevisionOferta rev = UCRevision.Revision;
            rev.IdOferta = idOferta;
            int idRevision = rev.Insert();

            foreach (ITipoMuestra item in UCRevision.lineasTipoMuestra)
            {
                TipoMuestraRevision tmp = new TipoMuestraRevision
                {
                    IdTipoMuestra = item.IdTipoMuestra,
                    IdRevision = idRevision
                };
                tmp.Insert();
            }

            foreach (ILineasParametros item in UCRevision.lineasParametros)
            {
                LineasRevisionOferta lr = new LineasRevisionOferta
                {
                    IdParametro = item.IdParametro,
                    Cantidad = item.Cantidad,
                    Metodo = item.Metodo,
                    IdRevisionOferta = idRevision
                };
                lr.Insert();
            }
        }

        private void addPeticion_Checked(object sender, RoutedEventArgs e)
        {
            UCPeticion?.SetEnabled(rWithPeticion.IsChecked ?? false);
        }
    }
}
