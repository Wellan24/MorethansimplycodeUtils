using Cartif.Logs;
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
using System.ComponentModel;
using Cartif.Expectation;
using GUI.Analisis;
using GenericForms.Implemented;

namespace GUI.Windows
{
    /// <summary>
    /// Lógica de interacción para Proecemientos.xaml
    /// </summary>
    public partial class PNTsBiomasa : MetroWindow
    {

        public static readonly DependencyProperty IconTitleProperty =
            DependencyProperty.Register("IconTitle", typeof(ImageSource), typeof(PNTsBiomasa), new PropertyMetadata(null));
        public ImageSource IconTitle
        {
            get { return (ImageSource)GetValue(IconTitleProperty); }
            set { SetValue(IconTitleProperty, value); }
        }

        public int IdMuestra { get; set; }

        public PNTsBiomasa(int idMuestra)
        {
            InitializeComponent();
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE;component/images/cabecera.png", UriKind.Absolute));
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            IdMuestra = idMuestra;
            CargarDatos();
        }

        private void CargarDatos()
        {

            int idTecnicoRecepcion = FactoriaMuestraRecepcionBiomasa.GetTecnico(IdMuestra);

            PageHumedad.Mediciones = FactoriaHumedadTotal.GetMediciones(IdMuestra) ?? FactoriaMedicionPNT.GetDefaults(idTecnicoRecepcion, IdMuestra, true);
            PageDensidad.Medicion = FactoriaDensidad.GetMedicion(IdMuestra) ?? FactoriaMedicionPNT.GetDefault(idTecnicoRecepcion, IdMuestra);
            PageDurabilidad.Medicion = FactoriaDurabilidad.GetMedicion(IdMuestra) ?? FactoriaMedicionPNT.GetDefault(idTecnicoRecepcion, IdMuestra);

            PageDimensiones.Medicion = FactoriaDimensionesPelet.GetMedicion(IdMuestra) ?? FactoriaMedicionPNT.GetDefault(idTecnicoRecepcion, IdMuestra);
            PageDimensiones.Dimensiones = FactoriaDimensionesPelet.GetParametro(PageDimensiones.Medicion.Id) ?? FactoriaDimensionesPelet.GetDefault();

            PageHumedad3.Mediciones = FactoriaHumedad3.GetMediciones(IdMuestra) ?? FactoriaMedicionPNT.GetDefaults(idTecnicoRecepcion, IdMuestra);
            PageCenizas.Medicion = FactoriaCenizas.GetMedicion(IdMuestra) ?? FactoriaMedicionPNT.GetDefault(idTecnicoRecepcion, IdMuestra);
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
            using (NpgsqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    Guardar(conn);
                    trans.Commit();
                    //trans.Rollback();
                    MessageBox.Show("Datos guardados con éxito");
                    RecargarHumedades();
                }
                catch (Exception ex)
                {
                    if (trans != null)
                        trans.Rollback();

                    CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "Error al guardar los datos de los análisis", ex);
                    MessageBox.Show("Error al guardar los datos de los análisis. Por favor, informa a soporte.");
                    this.Close();
                }
            }
        }

        private void Guardar(NpgsqlConnection conn)
        {
            GuardarMedicion(conn);
        }

        private void GuardarMedicion(NpgsqlConnection conn)
        {
            GuardarHumedadTotal(conn);

            MedicionPNT medDen = PageDensidad.Prueba.Medicion;
            Densidad den = PageDensidad.Prueba.Densidad;
            MedicionPNT medDenCCI = PageDensidad.CCI.Medicion;
            Densidad denCCI = PageDensidad.CCI.Densidad;
            List<EquipoMedicion> equiposDen = PageDensidad.Prueba.UCEquipos.listaEquipos.Children.OfType<TypePanel>().Select(p => p.InnerValue as EquipoMedicion).ToList();
            List<EquipoMedicion> equiposDenCCI = PageDensidad.CCI.UCEquipos.listaEquipos.Children.OfType<TypePanel>().Select(p => p.InnerValue as EquipoMedicion).ToList();
            GuardarDatos<Densidad, ReplicaDensidad, ControlDensidad>(conn, medDen, medDenCCI, den, denCCI, PageDensidad.CCI, "IdDensidad", equiposDen, equiposDenCCI);

            MedicionPNT medDur = PageDurabilidad.Prueba.Medicion;
            Finos fin = PageDurabilidad.Prueba.Finos;
            Durabilidad dur = PageDurabilidad.Prueba.Durabilidad;
            MedicionPNT medDurCCI = PageDurabilidad.CCI.Medicion;
            Finos finCCI = PageDurabilidad.CCI.Finos;
            Durabilidad durCCI = PageDurabilidad.CCI.Durabilidad;
            List<EquipoMedicion> equiposDur = PageDurabilidad.Prueba.UCEquipos.listaEquipos.Children.OfType<TypePanel>().Select(p => p.InnerValue as EquipoMedicion).ToList();
            List<EquipoMedicion> equiposDurCCI = PageDurabilidad.CCI.UCEquipos.listaEquipos.Children.OfType<TypePanel>().Select(p => p.InnerValue as EquipoMedicion).ToList();
            GuardarFinos(conn, medDur, fin, dur, medDurCCI, finCCI, durCCI, equiposDur, equiposDurCCI);

            GuardarTamanoPelets(conn);
            GuardarHumedad3(conn);

            MedicionPNT medCen = PageCenizas.Prueba.Medicion;
            Cenizas cen = PageCenizas.Prueba.Cenizas;
            MedicionPNT medCenCCI = PageCenizas.CCI.Medicion;
            Cenizas cenCCI = PageCenizas.CCI.Cenizas;
            List<EquipoMedicion> equiposCen=PageCenizas.Prueba.UCEquipos.listaEquipos.Children.OfType<TypePanel>().Select(p => p.InnerValue as EquipoMedicion).ToList();
            List<EquipoMedicion> equiposCenCCI=PageCenizas.CCI.UCEquipos.listaEquipos.Children.OfType<TypePanel>().Select(p => p.InnerValue as EquipoMedicion).ToList();
            GuardarDatos<Cenizas, ReplicaCeniza, ControlCenizas>(conn, medCen, medCenCCI, cen, cenCCI, PageCenizas.CCI, "IdCenizas", equiposCen, equiposCenCCI);
        }

        private void GuardarHumedadTotal(NpgsqlConnection conn)
        {
            /* Guardar mediciones y parámetros */
            MedicionHumedad[] controlesMedicionesHumedad = PageHumedad.listaMediciones.Children.OfType<MedicionHumedad>().ToArray();
            MedicionPNT medHum, medHumCCI;
            HumedadTotal hum, humCCI;
            List<EquipoMedicion> equiposHum, equiposHumCCI;

            foreach (MedicionHumedad control in controlesMedicionesHumedad)
            {
                medHum = control.Prueba.UCMedicion.Medicion;
                hum = control.Prueba.Humedad;
                medHumCCI = control.CCI.UCMedicion.Medicion;
                humCCI = control.CCI.Humedad;
                equiposHum=control.Prueba.UCEquipos.listaEquipos.Children.OfType<TypePanel>().Select(p => p.InnerValue as EquipoMedicion).ToList();
                equiposHumCCI=control.CCI.UCEquipos.listaEquipos.Children.OfType<TypePanel>().Select(p => p.InnerValue as EquipoMedicion).ToList();

                GuardarDatos<HumedadTotal, ReplicaHumedadTotal, ControlHumedad>(conn, medHum, medHumCCI, hum, humCCI, control.CCI, "IdHumedad", equiposHum, equiposHumCCI);
            }

            /* Borrado de mediciones eliminadas */
            MedicionPNT[] listaMedicionesBBDD = FactoriaHumedadTotal.GetMediciones(IdMuestra);
            if (listaMedicionesBBDD != null)
            {
                List<MedicionPNT> listaMedicionesBorrar = listaMedicionesBBDD.Where(m => !controlesMedicionesHumedad.Select(cm => cm.Prueba.Medicion).Any(me => me.Id == m.Id)).ToList();
                foreach (MedicionPNT medicion in listaMedicionesBorrar)
                {
                    medHum = medicion;
                    medHumCCI = FactoriaMedicionPNTcci.GetMedicion(medicion.Id);
                    if (medHumCCI != null)
                    {
                        BorrarEnlaceCCI(conn, medHum, medHumCCI);
                        humCCI = FactoriaHumedadTotal.GetParametro(medHumCCI.Id);
                        BorrarParam<HumedadTotal, ReplicaHumedadTotal>(conn, medHumCCI, humCCI);
                    }

                    hum = FactoriaHumedadTotal.GetParametro(medicion.Id);
                    BorrarParam<HumedadTotal, ReplicaHumedadTotal>(conn, medHum, hum);
                }
            }
        }

        private void GuardarFinos(NpgsqlConnection conn, MedicionPNT medDur, Finos fin, Durabilidad dur, MedicionPNT medDurCCI, Finos finCCI, Durabilidad durCCI, List<EquipoMedicion> equipos, List<EquipoMedicion> equiposCCI)
        {
            GuardarFinosParam(conn, medDur, fin, dur);
            GuardarEquipos(conn, equipos, medDur.Id);
            if (PageDurabilidad.CCI.Visibility == Visibility.Visible)
            {
                GuardarFinosParam(conn, medDurCCI, finCCI, durCCI);
                GuardarEnlaceCCI(conn, medDur, medDurCCI);
                GuardarEquipos(conn, equiposCCI, medDurCCI.Id);
            }
            else
            {
                if (BorrarEnlaceCCI(conn, medDur, medDurCCI))
                {
                    BorrarEquipos(conn, medDurCCI.Id);
                    BorrarFinosParam(conn, medDurCCI, finCCI, durCCI);
                    PageDurabilidad.CCI.BorrarMedicion();
                }
            }
        }

        private void GuardarFinosParam(NpgsqlConnection conn, MedicionPNT medDur, Finos fin, Durabilidad dur)
        {
            PersistenceDataManipulation.Guardar(conn, medDur);

            PersistenceDataManipulation.Guardar(conn, fin, medDur.Id, "IdMedicion");
            PersistenceDataManipulation.GuardarElement1N(conn, fin, fin.Replicas, p => p.Id, "IdFinos");
            PersistenceDataManipulation.Borrar1N(conn, fin.Replicas, fin.Id, "IdFinos");

            PersistenceDataManipulation.Guardar(conn, dur, medDur.Id, "IdMedicion");
            PersistenceDataManipulation.GuardarElement1N(conn, dur, dur.Replicas, p => p.Id, "IdDurabilidad");
            PersistenceDataManipulation.Borrar1N(conn, dur.Replicas, dur.Id, "IdDurabilidad");
        }

        private void BorrarFinosParam(NpgsqlConnection conn, MedicionPNT medDur, Finos fin, Durabilidad dur)
        {
            PersistenceDataManipulation.Borrar(conn, fin.Replicas);
            fin.Delete(conn);

            PersistenceDataManipulation.Borrar(conn, dur.Replicas);
            dur.Delete(conn);

            medDur.Delete(conn);
        }

        private void GuardarTamanoPelets(NpgsqlConnection conn)
        {
            MedicionPNT medTam = PageDimensiones.Medicion;
            DimensionesPelet tam = PageDimensiones.Dimensiones;
            List<EquipoMedicion> equiposTam = PageDimensiones.UCEquipos.listaEquipos.Children.OfType<TypePanel>().Select(p => p.InnerValue as EquipoMedicion).ToList();

            PersistenceDataManipulation.Guardar(conn, medTam);
            PersistenceDataManipulation.Guardar(conn, tam, medTam.Id, "IdMedicion");
            GuardarEquipos(conn, equiposTam, medTam.Id);

            ///* Guardado */
            PersistenceDataManipulation.GuardarElement1N(conn, tam, tam.Clases, t => t.Id, "IdDimension");
            PersistenceDataManipulation.GuardarListadoNN(conn, tam.Clases, cl => cl.Longitudes, cl => cl.Id, "IdClase");
            PersistenceDataManipulation.GuardarListadoNN(conn, tam.Clases, cl => cl.Diametros, cl => cl.Id, "IdClase");
            /* Borrado */
            PersistenceDataManipulation.BorrarListadoNN(conn, tam.Clases, cl => cl.Longitudes, cl => cl.Id, "IdClase");
            /*PersistenceDataManipulation.BorrarListadoNN(conn, tam.Clases, cl=>cl.Diametros, cl => cl.Id, "IdClase");*/ /*Diametros no se borran*/
            List<ClasePelet> listaClasesBorrar = PersistenceManager.SelectByProperty<ClasePelet>("IdDimension", tam.Id).Where(c => !tam.Clases.Exists(cl => cl.Id == c.Id)).ToList();
            PersistenceDataManipulation.BorrarListadoNN<ClasePelet, LongitudPelet>(conn, listaClasesBorrar, cl => cl.Id, "IdClase");
            PersistenceDataManipulation.BorrarListadoNN<ClasePelet, DiametroPelet>(conn, listaClasesBorrar, cl => cl.Id, "IdClase");
            PersistenceDataManipulation.Borrar(conn, listaClasesBorrar);
        }

        private void GuardarHumedad3(NpgsqlConnection conn)
        {
            /* Guardar mediciones y parámetros */
            MedicionHumedad3[] controlesMedicionesHumedad3 = PageHumedad3.listaMediciones.Children.OfType<MedicionHumedad3>().ToArray();
            MedicionPNT medHu3, medHu3CCI;
            Humedad3 hu3, hu3CCI;
            List<EquipoMedicion> equiposHu3, equiposHu3CCI;
            foreach (MedicionHumedad3 control in controlesMedicionesHumedad3)
            {
                medHu3 = control.Prueba.UCMedicion.Medicion;
                hu3 = control.Prueba.Humedad;
                medHu3CCI = control.CCI.UCMedicion.Medicion;
                hu3CCI = control.CCI.Humedad;
                equiposHu3 = control.Prueba.UCEquipos.listaEquipos.Children.OfType<TypePanel>().Select(p => p.InnerValue as EquipoMedicion).ToList();
                equiposHu3CCI = control.CCI.UCEquipos.listaEquipos.Children.OfType<TypePanel>().Select(p => p.InnerValue as EquipoMedicion).ToList();
                GuardarDatos<Humedad3, ReplicaHumedad3, ControlHumedad3>(conn, medHu3, medHu3CCI, hu3, hu3CCI, control.CCI, "IdHumedad", equiposHu3, equiposHu3CCI);
            }

            /* Borrado de mediciones eliminadas */
            MedicionPNT[] listaMedicionesBBDD = FactoriaHumedad3.GetMediciones(IdMuestra);
            if (listaMedicionesBBDD != null)
            {
                List<MedicionPNT> listaMedicionesBorrar = listaMedicionesBBDD.Where(m => !controlesMedicionesHumedad3.Select(cm => cm.Prueba.Medicion).Any(me => me.Id == m.Id)).ToList();
                foreach (MedicionPNT medicion in listaMedicionesBorrar)
                {
                    medHu3 = medicion;
                    medHu3CCI = FactoriaMedicionPNTcci.GetMedicion(medicion.Id);
                    if (medHu3CCI != null)
                    {
                        BorrarEnlaceCCI(conn, medHu3, medHu3CCI);
                        hu3CCI = FactoriaHumedad3.GetParametro(medHu3CCI.Id);
                        BorrarParam<Humedad3, ReplicaHumedad3>(conn, medHu3CCI, hu3CCI);
                    }

                    hu3 = FactoriaHumedad3.GetParametro(medicion.Id);
                    BorrarParam<Humedad3, ReplicaHumedad3>(conn, medHu3, hu3);
                }
            }
        }

        private void BorrarEquipos(NpgsqlConnection conn, int idMedicion)
        {
            PersistenceDataManipulation.Borrar1N<EquipoMedicion>(conn, null, idMedicion, "IdMedicion");
        }

        private void GuardarEquipos(NpgsqlConnection conn, List<EquipoMedicion> equipos, int idMedicion)
        {
            PersistenceDataManipulation.Guardar(conn, equipos, idMedicion, "IdMedicion");
        }

        /// <summary>
        /// Actualiza (guardar, actualizar y borrar) los datos de la medicion, parámetro, replicas y enlace entre medición del parámetro y medicion del CCI. Si la medición es eliminada solo borra.
        /// </summary>
        /// <typeparam name="T">Tipo genérico del parámetro a actualizar</typeparam>
        /// <typeparam name="T2">Tipo genérico de las replicas a actualizar</typeparam>
        /// <typeparam name="T3">Tipo genérico del UserControl del CCI (para en caso de no estar visible borrar los datos)</typeparam>
        /// <param name="conn">Conexión</param>
        /// <param name="medParam">Medición del parámetro</param>
        /// <param name="medParamCCI">Medición del CCI del parámetro</param>
        /// <param name="param">Parámetro</param>
        /// <param name="paramCCI">CCI del parámetro</param>
        /// <param name="control">UserControl del CCI</param>
        /// <param name="idFkReplica">Foreign key de la réplica respecto de su parámetro</param>
        private void GuardarDatos<T, T2, T3>(NpgsqlConnection conn, MedicionPNT medParam, MedicionPNT medParamCCI, T param, T paramCCI, T3 control, String idFkReplica, List<EquipoMedicion> equipos, List<EquipoMedicion> equiposCCI) where T : PersistenceData, IModelo, IReplicas<T2>
                                                                                                                                                     where T2 : PersistenceData, IModelo, ITipoReplicas
                                                                                                                                                     where T3 : Control, IMedicion
        {
            GuardarParam<T, T2>(conn, medParam, param, idFkReplica);
            GuardarEquipos(conn, equipos, medParam.Id);
            if (control.Visibility == Visibility.Visible)
            {
                GuardarParam<T, T2>(conn, medParamCCI, paramCCI, idFkReplica);
                GuardarEnlaceCCI(conn, medParam, medParamCCI);
                GuardarEquipos(conn, equiposCCI, medParamCCI.Id);
            }
            else
            {
                if (BorrarEnlaceCCI(conn, medParam, medParamCCI))
                {
                    BorrarEquipos(conn, medParamCCI.Id);
                    BorrarParam<T, T2>(conn, medParamCCI, paramCCI);
                    control.BorrarMedicion();
                }
            }
        }

        /// <summary>
        /// Borra los datos de la medición, parámetro y replicas.
        /// </summary>
        /// <typeparam name="T">Tipo genérico del parámetro a borrar</typeparam>
        /// <typeparam name="T2">Tipo genérico de las replicas a borrar</typeparam>
        /// <param name="conn">Conexión</param>
        /// <param name="medParam">Medición del parámetro a borrar</param>
        /// <param name="param">Parámetro a borrar</param>
        private void BorrarParam<T, T2>(NpgsqlConnection conn, MedicionPNT medParam, T param) where T : PersistenceData, IModelo, IReplicas<T2>
                                                                                              where T2 : PersistenceData, IModelo, ITipoReplicas
        {
            PersistenceDataManipulation.Borrar(conn, param.Replicas);
            param.Delete(conn);
            medParam.Delete(conn);
        }

        /// <summary>
        /// Actualiza los datos de la medicion, parámetro y replicas.
        /// </summary>
        /// <typeparam name="T">Tipo genérico del parámetro a actualizar</typeparam>
        /// <typeparam name="T2">Tipo genérico de las replicas a actualizar</typeparam>
        /// <param name="conn">Conexión</param>
        /// <param name="medParam">Medición del parámetro a actualizar</param>
        /// <param name="param">Parámetro a actualizar</param>
        /// <param name="idFkReplica">Foreign key de la réplica respecto de su parámetro</param>
        private void GuardarParam<T, T2>(NpgsqlConnection conn, MedicionPNT medParam, T param, String idFkReplica) where T : PersistenceData, IModelo, IReplicas<T2>
                                                                                               where T2 : PersistenceData, IModelo, ITipoReplicas
        {
            PersistenceDataManipulation.Guardar(conn, medParam);

            PersistenceDataManipulation.Guardar(conn, param, medParam.Id, "IdMedicion");
            PersistenceDataManipulation.GuardarElement1N(conn, param, param.Replicas, p => p.Id, idFkReplica); //IdFkReplica = "IdHumedad";
            PersistenceDataManipulation.Borrar1N(conn, param.Replicas, param.Id, idFkReplica);
        }

        private void GuardarEnlaceCCI(NpgsqlConnection conn, MedicionPNT medParam, MedicionPNT medParamCCI)
        {
            MedicionPNTcci medCCI = PersistenceManager.SelectByID<MedicionPNTcci>(medParam.Id);
            if (medCCI == null)
            {
                medCCI = new MedicionPNTcci() { Id = medParam.Id, IdCCI = medParamCCI.Id };
                medCCI.Insert(conn, false);
            }
        }

        private bool BorrarEnlaceCCI(NpgsqlConnection conn, MedicionPNT medParam, MedicionPNT medParamCCI)
        {
            MedicionPNTcci medCCI = PersistenceManager.SelectByID<MedicionPNTcci>(medParam.Id);
            if (medCCI != null)
            {
                medCCI.Delete(conn);
                return true;
            }
            return false;
        }

        private void RecargarHumedades()
        {
            PageDensidad.RecargarHumedad();
            PageCenizas.RecargarHumedad();
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Salir(sender, e);
        }

        private void Salir(object sender, CancelEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Deseas guardar antes de salir?", "Salir", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
                Guardar_Click(null, null);
            else if (messageBoxResult == MessageBoxResult.Cancel)
                e.Cancel = true;
        }

    }
}
