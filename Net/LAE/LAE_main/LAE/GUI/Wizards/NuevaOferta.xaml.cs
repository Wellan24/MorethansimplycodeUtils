using Dapper;
using GUI.Windows;
using LAE.Modelo;
using MahApps.Metro.Controls;
using Npgsql;
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
using System.Collections.ObjectModel;
using Cartif.Extensions;
using LAE.Clases;
using Cartif.Logs;

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
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE;component/images/cabecera.png", UriKind.Absolute));
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
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
                AnnoOferta = p.Fecha?.Year ?? DateTime.Now.Year,
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
                            AnnoOferta = DateTime.Now.Year
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
            UCPeticion.RecargarPagina();
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
                int idOferta = GuardarOferta();
                if (idOferta != 0)
                {
                    if (rWithPeticion.IsChecked ?? false)
                        GuardarPeticion(idOferta);

                    MessageBox.Show("Datos guardados con éxito");
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Se ha producido un error al guardar la oferta. Por favor, vuelve a intentarlo o informa a soporte.");
                    DialogResult = false;
                }
                this.Close();
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
                    if (rWithPeticion.IsChecked ?? false)
                    {
                        UCRevision.Revision = GenerarDatosRevisionDesdePeticion(UCPeticion.Peticion, UCOferta.Oferta);
                        UCRevision.CargarTipoMuestra(UCPeticion.lineasTipoMuestra);
                        UCRevision.CargarParametro(UCPeticion.lineasParametros);
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
                int idOferta = GuardarOferta();
                if (idOferta != 0)
                {
                    if (rWithPeticion.IsChecked ?? false)
                        GuardarPeticion(idOferta);

                    if (GuardarRevision(idOferta) != 0)
                    {

                        MessageBox.Show("Datos guardados con éxito");
                        DialogResult = true;
                    }
                    else
                    {
                        MessageBox.Show("Se ha producido un error al guardar la revisión. Por favor, vuelve a intentarlo o informa a soporte.");
                        DialogResult = false;
                    }
                }
                else
                {
                    MessageBox.Show("Se ha producido un error al guardar la oferta. Por favor, vuelve a intentarlo o informa a soporte.");
                    DialogResult = false;
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Datos erróneos. Por favor, revisa la información");
            }
        }

        private int GuardarOferta()
        {
            StringBuilder consulta = new StringBuilder("SELECT insertaroferta(@IdTecnico, @IdContacto, @IdCliente, @AnnoOferta)");
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<int>(consulta.ToString(), UCOferta.Oferta).FirstOrDefault();
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al guardar la oferta. Por favor, vuelve a intentarlo o informa a soporte.");
                return 0;
            }
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
                    IdPeticion = idPeticion
                };
                lp.Insert();
            }
        }

        private int GuardarRevision(int idOferta)
        {
            RevisionOferta rev = UCRevision.Revision;
            rev.IdOferta = idOferta;

            int idRevision;
            StringBuilder consulta = new StringBuilder("SELECT insertarrevision(@Observaciones, @FechaEmision, @Importe, @RequiereTomaMuestra, @LugarMuestra, @NumPuntosMuestreo, @TrabajoPuntual, @Frecuencia, @PlazoRealizacion, @IdOferta, @IdTecnico)");
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    idRevision = conn.Query<int>(consulta.ToString(), rev).FirstOrDefault();

                if (idRevision != 0)
                {
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
                            IdRevisionOferta = idRevision
                        };
                        lr.Insert();
                    }
                }
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al guardar la revisión. Por favor, vuelve a intentarlo o informa a soporte.");
                idRevision = 0;
            }

            return idRevision;
        }

        private void addPeticion_Checked(object sender, RoutedEventArgs e)
        {
            UCPeticion?.SetEnabled(rWithPeticion.IsChecked ?? false);
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

                if (((Hyperlink)sender).Name.Equals("LinkPeticion"))
                {
                    Peticion pet = Util.GenerarPeticionFromRevision(r, o);
                    UCPeticion.CargarNuevaPeticion(pet);
                    UCPeticion.CargarTipoMuestra(Util.GetTiposMuestraFromRevision(combo.idSeleccionado));
                    UCPeticion.CargarParametro(Util.GetParametrosFromRevision(combo.idSeleccionado));
                }
                else if (((Hyperlink)sender).Name.Equals("LinkRevision"))
                {
                    r.Id = 0;
                    r.IdOferta = 0;
                    r.Num = 1;
                    UCRevision.CargarNuevaRevision(r);
                    UCRevision.CargarTipoMuestra(Util.GetTiposMuestraFromRevision(combo.idSeleccionado));
                    UCRevision.CargarParametro(Util.GetParametrosFromRevision(combo.idSeleccionado));
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
