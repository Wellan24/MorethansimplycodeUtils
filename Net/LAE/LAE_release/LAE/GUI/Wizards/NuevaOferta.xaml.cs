using Dapper;
using GUI.Windows;
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
using System.Collections.ObjectModel;
using Cartif.Extensions;
using LAE.Comun.Clases;
using Cartif.Logs;
using GUI.Controls;
using LAE.Comun.Modelo;

namespace GUI.Wizards
{
    /// <summary>
    /// Lógica de interacción para NuevaPeticion.xaml
    /// </summary>
    public partial class NuevaOferta : MetroWindow
    {

        public static readonly DependencyProperty IconTitleProperty =
            DependencyProperty.Register("IconTitle", typeof(ImageSource), typeof(NuevaOferta), new PropertyMetadata(null));
        public ImageSource IconTitle
        {
            get { return (ImageSource)GetValue(IconTitleProperty); }
            set { SetValue(IconTitleProperty, value); }
        }


        public NuevaOferta()
        {
            InitializeComponent();
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE.Comun;component/images/cabecera.png", UriKind.Absolute));
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            CargarDatos();
        }

        private void CargarDatos()
        {
            Tecnico[] tecnicos = RecuperarTecnicos();

            CargarPeticion(tecnicos);
            CargarOferta(tecnicos);
            CargarRevision(tecnicos);
        }

        private void CargarPeticion(Tecnico[] tecnicos)
        {
            UCPeticion.Tecnicos = tecnicos;
            UCPeticion.Peticion = new Peticion()
            {
                Fecha = DateTime.Now
            };
        }

        private void CargarOferta(Tecnico[] tecnicos)
        {
            UCOferta.Tecnicos = tecnicos;
        }

        private void CargarRevision(Tecnico[] tecnicos)
        {
            UCRevision.Tecnicos = tecnicos;
        }

        private Oferta GenerarDatosOfertaDesdePeticion(Peticion p)
        {
            Oferta o = new Oferta
            {
                AnnoOferta = p.Fecha ?? DateTime.Now,
                IdCliente = p.IdCliente,
                IdContacto = p.IdContacto
            };

            return o;
        }

        private RevisionOferta GenerarDatosRevisionDesdePeticion(Peticion p, Oferta o)
        {
            RevisionOferta r = new RevisionOferta
            {
                Frecuencia = p.Frecuencia,
                LugarMuestra = p.LugarMuestra,
                NumPuntosMuestreo = p.NumPuntosMuestreo,
                PlazoRealizacion = p.PlazoRealizacion,
                RequiereTomaMuestra = p.RequiereTomaMuestra,
                TrabajoPuntual = p.TrabajoPuntual,
                Observaciones = p.Observaciones,
            };
            r.FechaEmision = DateTime.Now;
            r.Num = 1;
            return r;
        }

        private void bNext1_Click(object sender, RoutedEventArgs e)
        {
            if ((!rWithPeticion.IsChecked ?? false) || UCPeticion.ValidarPeticion())
            {
                if (UCOferta.Oferta == default(Oferta))
                {
                    if (rWithPeticion.IsChecked ?? false)
                        UCOferta.Oferta = GenerarDatosOfertaDesdePeticion(UCPeticion.Peticion);
                    else
                        UCOferta.Oferta = new Oferta
                        {
                            AnnoOferta = DateTime.Now
                        };
                }
                else
                {
                    UCOferta.RecargarPagina();
                }
                Tab2.IsSelected = true;
            }
            else
            {
                MessageBox.Show("Datos erróneos. Por favor, revisa la información");
            }
        }

        private void bPrev1_Click(object sender, RoutedEventArgs e)
        {
            UCPeticion.RecargarPagina(); /* necesito recargar por si añadi nuevos contactos o clientes */
            Tab1.IsSelected = true;
        }

        private void bPrev2_Click(object sender, RoutedEventArgs e)
        {
            UCOferta.RecargarPagina();
            Tab2.IsSelected = true;
        }



        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Estás seguro que deseas cancelar? En caso afirmativo, la información no se guadará", "Cancelar", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                DialogResult = false;
                this.Close();
            }
        }

        private Tecnico[] RecuperarTecnicos()
        {
            return PersistenceManager.SelectAll<Tecnico>().OrderBy(t => t.Nombre).ToArray();
        }

        private void bGuardar1_Click(object sender, RoutedEventArgs e)
        {

            if (UCOferta.ValidarOferta())
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                {
                    NpgsqlTransaction trans = null;
                    try
                    {
                        trans = conn.BeginTransaction();
                        int idOferta = GuardarOferta(conn);
                        if (idOferta == 0)
                            throw new Exception("Error guardar oferta");
                        if (rWithPeticion.IsChecked ?? false)
                            GuardarPeticion(conn, idOferta);

                        trans.Commit();
                        DialogResult = true;
                        MessageBox.Show("Datos guardados con éxito");
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        if (trans != null)
                            trans.Rollback();

                        CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "Error al guardar petición y oferta", ex);
                        MessageBox.Show("Se ha producido un error al intentar guardar los datos. Por favor, recargue la página o informa a soporte.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Datos erróneos. Por favor, revisa la información");
            }
        }

        private void bNext2_Click(object sender, RoutedEventArgs e)
        {
            if (UCOferta.ValidarOferta())
            {
                if (UCRevision.Revision == default(RevisionOferta))
                {
                    if (rWithPeticion.IsChecked == true)
                    {
                        UCRevision.Revision = GenerarDatosRevisionDesdePeticion(UCPeticion.Peticion, UCOferta.Oferta);
                        UCRevision.CargarTipoMuestra(UCPeticion.LineasTipoMuestra);
                        UCRevision.CargarParametros(UCPeticion.LineasPuntosControl);
                    }
                    else
                        UCRevision.Revision = new RevisionOferta()
                        {
                            FechaEmision = DateTime.Now,
                            Num = 1,
                        };
                }
                else
                {
                    UCRevision.RecargarPagina();
                }
                Tab3.IsSelected = true;

            }
            else
            {
                MessageBox.Show("Datos erróneos. Por favor, revisa la información");
            }
        }

        private void bGuardar2_Click(object sender, RoutedEventArgs e)
        {

            if (UCRevision.ValidarRevision())
            {

                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                using (NpgsqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        int idOferta = GuardarOferta(conn);
                        if (idOferta == 0)
                            throw new Exception("Error al guardar la oferta");
                        if (rWithPeticion.IsChecked ?? false)
                            GuardarPeticion(conn, idOferta);
                        GuardarRevision(conn, idOferta);

                        trans.Commit();
                        DialogResult = true;
                        MessageBox.Show("Datos guardados con éxito");
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        if (trans != null)
                            trans.Rollback();

                        CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "Error al guardar petición y oferta", ex);
                        MessageBox.Show("Se ha producido un error al intentar guardar los datos. Por favor, recargue la página o informa a soporte.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Datos erróneos. Por favor, revisa la información");
            }
        }

        private int GuardarOferta(NpgsqlConnection conn)
        {
            StringBuilder consulta = new StringBuilder("SELECT insertaroferta(@IdTecnico, @IdContacto, @IdCliente, @AnnoOferta)");
            return conn.Query<int>(consulta.ToString(), UCOferta.Oferta).FirstOrDefault();
        }

        private void GuardarPeticion(NpgsqlConnection conn, int idOferta)
        {
            Peticion pet = UCPeticion.Peticion;
            pet.IdOferta = idOferta;
            int idPeticion = pet.Insert(conn);
            if(idPeticion==0)
                throw new Exception("Error guardar la petición");

            foreach (ITipoMuestra item in UCPeticion.LineasTipoMuestra)
            {
                TipoMuestraPeticion tmp = new TipoMuestraPeticion
                {
                    IdTipoMuestra = item.IdTipoMuestra,
                    IdPeticion = idPeticion
                };
                int n = tmp.Insert(conn);
                if (n == 0)
                    throw new Exception("Error guardar tipos de muestra");
            }

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
                int idPuntoControl = pcp.Insert(conn);
                if (idPuntoControl == 0)
                    throw new Exception("Error guardar punto de control");
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
                        throw new Exception("Error guardar lineas de parámetros");
                }
            }
        }

        private void GuardarRevision(NpgsqlConnection conn, int idOferta)
        {
            RevisionOferta rev = UCRevision.Revision;
            rev.IdOferta = idOferta;

            StringBuilder consulta = new StringBuilder("SELECT insertarrevision(@Observaciones, @FechaEmision, @Importe, @RequiereTomaMuestra, @LugarMuestra, @NumPuntosMuestreo, @TrabajoPuntual, @Frecuencia, @PlazoRealizacion, @IdOferta, @IdTecnico)");
            int idRevision= conn.Query<int>(consulta.ToString(), rev).FirstOrDefault();
            if (idRevision == 0)
                throw new Exception("Error guardar la petición");

            foreach (ITipoMuestra item in UCRevision.LineasTipoMuestra)
            {
                TipoMuestraRevision tmr = new TipoMuestraRevision
                {
                    IdTipoMuestra = item.IdTipoMuestra,
                    IdRevision = idRevision
                };
                int n = tmr.Insert(conn);
                if (n == 0)
                    throw new Exception("Error guardar tipos de muestra");
            }

            foreach (ControlLineaParametro clp in UCRevision.LineasPuntosControl)
            {
                IPuntoControl ipc = clp.PuntoControl;
                PuntocontrolRevision pcr = new PuntocontrolRevision
                {
                    Nombre = ipc.Nombre,
                    Observaciones = ipc.Observaciones,
                    Importe = ipc.Importe,
                    IdRevision = idRevision
                };
                int idPuntoControl = pcr.Insert(conn);
                if (idPuntoControl == 0)
                    throw new Exception("Error guardar punto de control");
                foreach (ILineasParametros ilp in ipc.Lineas)
                {
                    LineaRevisionOferta lr = new LineaRevisionOferta
                    {
                        Cantidad = ilp.Cantidad,
                        IdParametro = ilp.IdParametro,
                        IdPControlRevisionOferta = idPuntoControl
                    };
                    int n = lr.Insert(conn);
                    if (n == 0)
                        throw new Exception("Error guardar lineas de parámetros");
                }
            }


        }

        private void addPeticion_Checked(object sender, RoutedEventArgs e)
        {
            UCPeticion?.SetEnabled(rWithPeticion.IsChecked ?? false);
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

                if (((Hyperlink)sender).Name.Equals("LinkPeticion"))
                {
                    Peticion pet = LAE.Clases.Util.GenerarPeticionFromRevision(r, o);
                    UCPeticion.CargarNuevaPeticion(pet);
                    UCPeticion.CargarTipoMuestra(LAE.Clases.Util.GetTiposMuestraFromRevision(combo.idSeleccionado));
                    UCPeticion.CargarParametros(combo.idSeleccionado);
                }
                else if (((Hyperlink)sender).Name.Equals("LinkRevision"))
                {
                    r.Id = 0;
                    r.IdOferta = 0;
                    r.Num = 1;
                    UCRevision.CargarNuevaRevision(r);
                    UCRevision.CargarTipoMuestra(LAE.Clases.Util.GetTiposMuestraFromRevision(combo.idSeleccionado));
                    UCRevision.CargarParametros(combo.idSeleccionado);
                }
                MessageBox.Show("Datos cargados con éxito");
            }

        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Util.VisualizeWindow(this);
        }
    }
}
