using Cartif.Logs;
using GUI.Controls;
using LAE.Comun.Clases;
using LAE.Modelo;
using MahApps.Metro.Controls;
using Npgsql;
using LAE.Comun.Persistence;
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
using LAE.Comun.Modelo;

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

        public Peticiones(Cliente[] clientes, Tecnico[] tecnicos, Peticion peticion)
        {
            InitializeComponent();
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE.Comun;component/images/cabecera.png", UriKind.Absolute));
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

            CargarDatos(clientes, tecnicos, peticion);
        }

        public Peticiones(Cliente[] clientes, Tecnico[] tecnicos, Peticion peticion, Oferta o)
        {
            InitializeComponent();
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE.Comun;component/images/cabecera.png", UriKind.Absolute));

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
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                using (NpgsqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        Peticion pet = UCPeticion.Peticion;
                        int idPeticion = GuardarPeticion(conn, pet);
                        GuardarTipoMuestra(conn, idPeticion);
                        GuardarPuntosControl(conn, idPeticion);

                        trans.Commit();
                        DialogResult = true;
                        MessageBox.Show("Datos guardados con éxito");
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        if (trans != null)
                            trans.Rollback();

                        CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "Error al guardar la petición", ex);
                        MessageBox.Show("Se ha producido un error al intentar guardar los datos. Por favor, recargue la página o informa a soporte.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Datos erróneos. Por favor, revisa la información");
            }

        }

        private int GuardarPeticion(NpgsqlConnection conn, Peticion pet)
        {

            if (pet.Id == 0)
            {
                int idPeticion = pet.Insert(conn);
                if (idPeticion == 0)
                    throw new PersistenceDataException("Error al guardar la petición");
                return idPeticion;
            }

            if (!pet.Update(conn))
                throw new PersistenceDataException("Error al actualizar la petición");
            return pet.Id;
        }

        private void GuardarTipoMuestra(NpgsqlConnection conn, int idPeticion)
        {
            List<TipoMuestraPeticion> lineas = PersistenceManager.SelectByProperty<TipoMuestraPeticion>("IdPeticion", idPeticion).ToList();

            foreach (ITipoMuestra item in UCPeticion.LineasTipoMuestra)
            {
                TipoMuestraPeticion tmp = new TipoMuestraPeticion();
                tmp.IdTipoMuestra = item.IdTipoMuestra;

                if (item.Id == 0)
                {
                    /* inserto nuevos */
                    tmp.IdPeticion = idPeticion;
                    int n = tmp.Insert(conn);
                    if (n == 0)
                        throw new PersistenceDataException("Error al guardar los tipos de muestra");
                }
                else
                {
                    /* actualizo existentes */
                    tmp.Id = item.Id;
                    tmp.IdPeticion = item.IdRelacion;
                    if (!tmp.Update(conn))
                        throw new PersistenceDataException("Error al actualizar los tipos de muestra");

                    lineas.Remove(tmp);
                }
            }
            foreach (TipoMuestraPeticion item in lineas)
            {
                /* elimino borrados */
                if (!item.Delete(conn))
                    throw new PersistenceDataException("Error al borrar los tipos de muestra");
            }
        }

        private void GuardarPuntosControl(NpgsqlConnection conn, int idPeticion)
        {

            List<PuntocontrolPeticion> puntosControlBBDD = PersistenceManager.SelectByProperty<PuntocontrolPeticion>("IdPeticion", idPeticion).ToList();
            foreach (ControlLineaParametro clp in UCPeticion.LineasPuntosControl)
            {
                IPuntoControl ipc = clp.PuntoControl;
                PuntocontrolPeticion pcp = new PuntocontrolPeticion
                {
                    Nombre = ipc.Nombre,
                    Observaciones = ipc.Observaciones,
                    Importe = ipc.Importe,
                    IdPeticion = idPeticion
                };
                if (ipc.Id == 0)
                {
                    /* inserto nuevo*/
                    int idPuntoControl = pcp.Insert(conn);
                    if (idPuntoControl == 0)
                        throw new PersistenceDataException("Error al insertar puntos de control");
                    GuardarParametros(conn, ipc, idPuntoControl);
                }
                else
                {
                    /* actualizo */
                    ActualizarParametros(conn, ipc);
                    pcp.Id = ipc.Id;
                    if (!pcp.Update(conn))
                        throw new PersistenceDataException("Error al actualizar puntos de control");
                    puntosControlBBDD.Remove(pcp);
                }
            }

            foreach (PuntocontrolPeticion pcp in puntosControlBBDD)
            {
                /* elimino borrados */
                EliminarParametros(conn, pcp);
                if (!pcp.Delete(conn))
                    throw new PersistenceDataException("Error al borrar puntos de control");
            }
        }

        private void ActualizarParametros(NpgsqlConnection conn, IPuntoControl ipc)
        {
            List<LineaPeticion> lineas = PersistenceManager.SelectByProperty<LineaPeticion>("IdPControlPeticion", ipc.Id).ToList();

            foreach (ILineasParametros ilp in ipc.Lineas)
            {

                LineaPeticion lp = new LineaPeticion
                {
                    Cantidad = ilp.Cantidad,
                    IdParametro = ilp.IdParametro,
                    IdPControlPeticion = ipc.Id
                };

                if (ilp.Id == 0)
                {
                    int n = lp.Insert(conn);
                    if (n == 0)
                        throw new PersistenceDataException("Error al guardar los parámetros");
                }
                else {
                    /* no hay update */
                    lp.Id = ilp.Id;
                    lineas.Remove(lp);
                }
            }

            foreach (LineaPeticion lp in lineas)
            {
                if (!lp.Delete(conn))
                    throw new PersistenceDataException("Error al borrar los parámetros");
            }
        }

        private void GuardarParametros(NpgsqlConnection conn, IPuntoControl ipc, int idPuntoControl)
        {
            foreach (ILineasParametros ilp in ipc.Lineas)
            {
                LineaPeticion lp = new LineaPeticion
                {
                    Cantidad = ilp.Cantidad,
                    IdParametro = ilp.IdParametro,
                    IdPControlPeticion = idPuntoControl
                };
                int n = lp.Insert(conn);
                if (n == 0)
                    throw new PersistenceDataException("Error al guardar parámetros");
            }
        }

        private void EliminarParametros(NpgsqlConnection conn, PuntocontrolPeticion pcp)
        {
            List<LineaPeticion> lineas = PersistenceManager.SelectByProperty<LineaPeticion>("IdPControlPeticion", pcp.Id).ToList();
            foreach (LineaPeticion lp in lineas)
            {
                if (!lp.Delete(conn))
                    throw new PersistenceDataException("Error al borrar parámetros");
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
            combo.Lista = LAE.Clases.Util.ComboBoxCodigoOfertas();
            combo.Owner = Window.GetWindow(this);
            combo.ShowDialog();

            if (combo.DialogResult ?? false)
            {
                RevisionOferta r = PersistenceManager.SelectByID<RevisionOferta>(combo.idSeleccionado);
                Oferta o = LAE.Clases.Util.GetOfertaFromRevision(combo.idSeleccionado);

                Peticion pet = LAE.Clases.Util.GenerarPeticionFromRevision(r, o);
                UCPeticion.CargarNuevaPeticion(pet);
                UCPeticion.CargarTipoMuestra(LAE.Clases.Util.GetTiposMuestraFromRevision(combo.idSeleccionado));
                UCPeticion.CargarParametros(combo.idSeleccionado);

                MessageBox.Show("Datos cargados con éxito");
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Util.VisualizeWindow(this);
        }
    }
}
