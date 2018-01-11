using Cartif.Extensions;
using Cartif.Logs;
using LAE.Modelo;
using MahApps.Metro.Controls;
using Npgsql;
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
using System.Windows.Shapes;

namespace GUI.Windows
{
    /// <summary>
    /// Lógica de interacción para GestionMuestras.xaml
    /// </summary>
    public partial class GestionMuestras : MetroWindow
    {
        public static readonly DependencyProperty IconTitleProperty =
            DependencyProperty.Register("IconTitle", typeof(ImageSource), typeof(GestionMuestras), new PropertyMetadata(null));
        public ImageSource IconTitle
        {
            get { return (ImageSource)GetValue(IconTitleProperty); }
            set { SetValue(IconTitleProperty, value); }
        }

        private Oferta oferta;
        private Trabajo trabajo;
        private RevisionOferta revision;

        private TomaMuestraAgua tmAgua;
        private RecepcionAgua recepAgua;
        private RecepcionBiomasa recepBiomasa;


        public GestionMuestras(Oferta o, Trabajo t)
        {
            InitializeComponent();
            oferta = o;
            trabajo = t;
            revision = GetRevisionAceptada();
            MostrarEnlaces();
        }

        private RevisionOferta GetRevisionAceptada()
        {
            return revision = PersistenceManager.SelectByProperty<RevisionOferta>("IdOferta", oferta.Id).Where(r => r.Aceptada == true).FirstOrDefault();
        }

        private void MostrarEnlaces()
        {
            EnlacesAguas();
            EnlacesBiomasa();
        }

        private void EnlacesAguas()
        {
            /* agua */
            if (revision.ExisteTipoMuestra(FactoriaRevisionesOferta.AGUA))
            {
                if (revision.ExisteTomaMuestra())
                {
                    tmAgua = PersistenceManager.SelectByProperty<TomaMuestraAgua>("IdTrabajo", trabajo.Id).FirstOrDefault();
                    tomaMuestraAgua.Content = (tmAgua != null ? "Editar " : "Nueva ") + tomaMuestraAgua.Tag;
                    if (tmAgua == null)
                    {
                        //borrarTomaMuestraAgua.Visibility = Visibility.Collapsed;
                        recepcionAgua.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        //borrarTomaMuestraAgua.Visibility = Visibility.Visible;
                        recepcionAgua.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    tomaMuestraAgua.Visibility = Visibility.Collapsed;
                    //borrarTomaMuestraAgua.Visibility = Visibility.Collapsed;
                }

                recepAgua = PersistenceManager.SelectByProperty<RecepcionAgua>("IdTrabajo", trabajo.Id).FirstOrDefault();
                recepcionAgua.Content = (recepAgua != null ? "Editar " : "Nueva ") + recepcionAgua.Tag;
                //if (recepAgua == null)
                //    borrarRecepcionAgua.Visibility = Visibility.Collapsed;
                //else
                //    borrarRecepcionAgua.Visibility = Visibility.Visible;
            }
            else
            {
                tomaMuestraAgua.Visibility = Visibility.Collapsed;
                //borrarTomaMuestraAgua.Visibility = Visibility.Collapsed;
                recepcionAgua.Visibility = Visibility.Collapsed;
                //borrarRecepcionAgua.Visibility = Visibility.Collapsed;
            }
        }

        private void EnlacesBiomasa()
        {
            if (revision.ExisteTipoMuestra(FactoriaRevisionesOferta.BIOMASA))
            {
                recepBiomasa = PersistenceManager.SelectByProperty<RecepcionBiomasa>("IdTrabajo", trabajo.Id).FirstOrDefault();
                recepcionBiomasa.Content = (recepBiomasa != null ? "Editar " : "Nueva ") + recepcionBiomasa.Tag;
                //if (recepBiomasa == null)
                //    borrarRecepcionBiomasa.Visibility = Visibility.Collapsed;
                //else
                //    borrarRecepcionBiomasa.Visibility = Visibility.Visible;
            }
            else
            {
                recepcionBiomasa.Visibility = Visibility.Collapsed;
                //borrarRecepcionBiomasa.Visibility = Visibility.Collapsed;
            }
        }

        private void TomaMuestraAgua_Click(object sender, RoutedEventArgs e)
        {
            TomasMuestraAgua ventanaTomasMuestra = new TomasMuestraAgua(oferta, trabajo, tmAgua ?? new TomaMuestraAgua() { IdTrabajo = trabajo.Id });
            ventanaTomasMuestra.Owner = this;
            ventanaTomasMuestra.ShowDialog();
            EnlacesAguas();
        }

        /* NO SE SI PERMITIDO BORRAR POR EL TEMA DE LOS IDS GENERADOS */
        //private void BorrarTomaMuestraAgua_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBoxResult messageBoxResult = MessageBox.Show("¿Estas seguro que deseas eliminar la toma de muestra? Una vez eliminada, sus datos desaparecerán definitavamente", "Borrar Toma de Muestra", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
        //    if (messageBoxResult == MessageBoxResult.Yes)
        //    {
        //        MessageBox.Show("Cuando existan ensayos debere comprobar estos previamente (parte por desarrollar). También debere validar si hay muestra asociada");
        //        if (tmAgua.NumBlanco == null || tmAgua.LastNumBlanco())
        //        {
        //            /* muestras */
        //            MuestraAgua[] muestras = PersistenceManager.SelectByProperty<MuestraAgua>("IdTomaMuestra", tmAgua.Id).ToArray();

        //            using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
        //            using (NpgsqlTransaction trans = conn.BeginTransaction())
        //            {
        //                try
        //                {
        //                    BorrarDatosTMAgua(conn, tmAgua, muestras);
        //                    trans.Commit();
        //                    EnlacesAguas();
        //                    MessageBox.Show("Toma de muestra borrada con éxito");
        //                }
        //                catch (Exception ex)
        //                {
        //                    if (trans != null)
        //                        trans.Rollback();

        //                    CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "Error al borrar la toma de muestra", ex);
        //                    MessageBox.Show("Se ha producido un error al intentar borrar los datos. Por favor, recargue la página o informa a soporte.");
        //                }
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("No se puede borrar la toma de muestra porque tiene asociado un código de Blanco de Muestreo, y existen códigos de Blanco de Muestreo posteriores al código asignado en la Toma de Muestra a borrar");
        //        }
        //    }
        //}

        //private void BorrarDatosTMAgua(NpgsqlConnection conn, TomaMuestraAgua tmAgua, MuestraAgua[] muestras)
        //{
        //    /* listas de cada muestra (materiales, localizaciones, ...) */
        //    PersistenceDataManipulation.BorrarListadoNN<MuestraAgua, MaterialesMuestraAgua>(conn, muestras, ma => ma.Id, "IdMuestra");
        //    PersistenceDataManipulation.BorrarListadoNN<MuestraAgua, LocalizacionesMuestraAgua>(conn, muestras, ma => ma.Id, "IdMuestra");
        //    PersistenceDataManipulation.BorrarListadoNN<MuestraAgua, ParamsInsituMuestraAgua>(conn, muestras, ma => ma.Id, "IdMuestra");
        //    PersistenceDataManipulation.BorrarListadoNN<MuestraAgua, ParamsLaboratorioMuestraAgua>(conn, muestras, ma => ma.Id, "IdMuestra");
        //    PersistenceDataManipulation.BorrarListadoNN<MuestraAgua, AlicuotaMuestraAgua>(conn, muestras, ma => ma.Id, "IdMuestra");
        //    PersistenceDataManipulation.BorrarListadoNN<MuestraAgua, EquiposMuestraAgua>(conn, muestras, ma => ma.Id, "IdMuestra");

        //    /* elemento de cada muestra (tipo de muestra)*/
        //    PersistenceDataManipulation.BorrarListadoNN<MuestraAgua, TiposMuestraMuestraAgua>(conn, muestras, ma => ma.Id, "IdMuestra");

        //    /* lista de muestra de agua */
        //    PersistenceDataManipulation.BorrarListado1N<TomaMuestraAgua, MuestraAgua>(conn, tmAgua, tm => tm.Id, "IdTomaMuestra");

        //    /* blancos de muestreo */
        //    PersistenceDataManipulation.BorrarListado1N<TomaMuestraAgua, BlancomuestreoTMAgua>(conn, tmAgua, tm => tm.Id, "IdTomaMuestra");

        //    /* toma de muestra */
        //    tmAgua.Delete(conn);
        //}

        private void RecpecionAgua_Click(object sender, RoutedEventArgs e)
        {

            RecepcionesMuestraAgua ventanaRecepcionAgua = new RecepcionesMuestraAgua(oferta.IdCliente, trabajo.IdContacto, recepAgua ?? new RecepcionAgua() { IdTrabajo = trabajo.Id, IdTecnico = trabajo.IdTecnico });
            ventanaRecepcionAgua.Owner = this;
            ventanaRecepcionAgua.ShowDialog();
            EnlacesAguas();
        }

        /* NO SE SI PERMITIDO BORRAR POR EL TEMA DE LOS IDS GENERADOS */
        //private void BorrarRecpecionAgua_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBoxResult messageBoxResult = MessageBox.Show("¿Estas seguro que deseas eliminar la recepción de la muestra? Una vez eliminada, sus datos desaparecerán definitavamente", "Borrar Recepción de Muestra", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
        //    if (messageBoxResult == MessageBoxResult.Yes)
        //    {
        //        MessageBox.Show("Cuando existan ensayos debere comprobar estos previamente (parte por desarrollar).");

        //        /* muestras */
        //        MuestraRecepcionAgua[] muestras = PersistenceManager.SelectByProperty<MuestraRecepcionAgua>("IdRecepcion", recepAgua.Id).ToArray();

        //        using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
        //        using (NpgsqlTransaction trans = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                BorrarDatosRecepAgua(conn, recepAgua, muestras);
        //                trans.Commit();
        //                EnlacesAguas();
        //                MessageBox.Show("Toma de muestra borrada con éxito");
        //            }
        //            catch (Exception ex)
        //            {
        //                if (trans != null)
        //                    trans.Rollback();

        //                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "Error al borrar la toma de muestra", ex);
        //                MessageBox.Show("Se ha producido un error al intentar borrar los datos. Por favor, recargue la página o informa a soporte.");
        //            }
        //        }
        //    }
        //}

        //private void BorrarDatosRecepAgua(NpgsqlConnection conn, RecepcionAgua recepAgua, MuestraRecepcionAgua[] muestras)
        //{

        //    /* listas de cada muestra (alicuotas) y sublistas (lineas de alicuota) */
        //    muestras.ForEach(m =>
        //    {
        //        m.Alicuotas = new ObservableCollection<AlicuotaRecepcionAgua>();
        //        m.LoadData();
        //        PersistenceDataManipulation.BorrarListadoNN<AlicuotaRecepcionAgua, LineaAliRecepcionAgua>(conn, m.Alicuotas, a => a.Id, "IdAlicuota");
        //    });

        //    PersistenceDataManipulation.BorrarListadoNN<MuestraRecepcionAgua, AlicuotaRecepcionAgua>(conn, muestras, ma => ma.Id, "IdMuestra");

        //    /* lista de muestra de agua */
        //    PersistenceDataManipulation.BorrarListado1N<RecepcionAgua, MuestraRecepcionAgua>(conn, recepAgua, re => re.Id, "IdRecepcion");

        //    /* recepción de agua */
        //    recepAgua.Delete(conn);
        //}

        private void RecepcionBiomasa_Click(object sender, RoutedEventArgs e)
        {
            RecepcionesMuestraBiomasa ventanaRecepcionBiomasa = new RecepcionesMuestraBiomasa(oferta.IdCliente, trabajo.IdContacto, recepBiomasa ?? new RecepcionBiomasa() { IdTrabajo = trabajo.Id, IdTecnico = trabajo.IdTecnico });
            ventanaRecepcionBiomasa.Owner = this;
            ventanaRecepcionBiomasa.ShowDialog();
            EnlacesBiomasa();
        }







        private void PlanMedicion_Click(object sender, RoutedEventArgs e)
        {
            //PlanesMedicion ventanaPlanMedicion = new PlanesMedicion(oferta, trabajo, tm);
            //TomasMuestraAgua ventanaTomasMuestra = new TomasMuestraAgua(oferta, trabajo, tmAgua ?? new TomaMuestraAgua() { IdTrabajo = trabajo.Id });
        }
    }
}
