using Cartif.Logs;
using LAE.Biomasa.Controles;
using LAE.Biomasa.Modelo;
using LAE.Biomasa.Pages;
using LAE.Comun.Modelo.Procedimientos;
using LAE.Comun.Persistence;
using MahApps.Metro.Controls;
using Npgsql;
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

namespace LAE.Biomasa.Ventanas
{
    /// <summary>
    /// Lógica de interacción para WindowEquipoFus.xaml
    /// </summary>
    public partial class WindowEquipoFus : MetroWindow
    {
        public static readonly DependencyProperty IconTitleProperty =
               DependencyProperty.Register("IconTitle", typeof(ImageSource), typeof(WindowEquipoFus), new PropertyMetadata(null));
        public ImageSource IconTitle
        {
            get { return (ImageSource)GetValue(IconTitleProperty); }
            set { SetValue(IconTitleProperty, value); }
        }

        private EnsayoPNT ensayo;
        public EnsayoPNT Ensayo
        {
            get { return ensayo; }
            set
            {
                ensayo = value;
                if (Ensayo.Id != 0)
                    PageAnalisisEquipo.Visibility = Visibility.Visible;
                PageEnsayoEquipoFus.Ensayo = Ensayo;

                EquipoFusEventos();
                
                MuestraAnalisis[] muestras = GetMuestrasAnalisis();
                Analisis[] analisis = GetAnalisis();
                PageAnalisisEquipo.SetDataPage(muestras, analisis, Ensayo, true);
            }
        }

        public WindowEquipoFus()
        {
            InitializeComponent();
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE.Comun;component/images/cabecera.png", UriKind.Absolute));
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        private MuestraAnalisis[] GetMuestrasAnalisis()
        {
            MuestraRecepcionBiomasa[] muestras = FactoriaMuestraRecepcionBiomasa.GetMuestrasEnsayoFusibilidad(Ensayo.Id);

            /*Obtengo todos materiales, porque son muy pocos y no hacen la query lenta*/
            FusibilidadMaterialesreferencia[] materiales = PersistenceManager.SelectAll<FusibilidadMaterialesreferencia>().ToArray();

            List<MuestraAnalisis> muestrasCHN = muestras.Select(m => new MuestraAnalisis()
            {
                Id = m.Id,
                MaterialReferencia = false,
                Nombre = m.GetCodigoLae
            }).ToList();

            muestrasCHN.AddRange(materiales.Select(m => new MuestraAnalisis()
            {
                Id = m.Id,
                MaterialReferencia = true,
                Nombre = m.ToString()
            }));


            return muestrasCHN.OrderBy(m => m.Id).ToArray();
        }

        private Analisis[] GetAnalisis()
        {
            MuestraEnsayo[] muestrasEnsayo = PersistenceManager.SelectByProperty<MuestraEnsayo>("IdEnsayo", Ensayo.Id).ToArray();
            FusibilidadControl[] controlesCalidad = PersistenceManager.SelectByProperty<FusibilidadControl>("IdEnsayo", Ensayo.Id).ToArray();

            List<Analisis> lista = muestrasEnsayo.Select(l => new Analisis()
            {
                Id = l.Id,
                IdMuestra = l.IdMuestra,
                Orden = l.OrdenEnsayo,
                //IdHumedad = l.IdHumedad,
                //IdHumedad3 = l.IdHumedad3,
                CCI = false
            }).ToList();

            lista.AddRange(controlesCalidad.Select(c => new Analisis()
            {
                Id = c.Id,
                IdMuestra = c.IdMaterialReferencia,
                Orden = c.OrdenEnsayo,
                IdTecnico = c.IdTecnico,
                CCI = true
            }));
            return lista.OrderBy(l => l.Orden).ToArray();
        }

        private void EquipoFusEventos()
        {
            PageEnsayoEquipoFus.VisibilidadAnalisis += (s, e) =>
            {
                if (Ensayo.Id != 0)
                    PageAnalisisEquipo.Visibility = Visibility.Visible;
            };

            PageAnalisisEquipo.VerAnalisis += (s, e) =>
            {
                Analisis analisis = PageAnalisisEquipo.gridAnalisis.DataGrid.SelectedItem as Analisis;
                if (analisis == null || analisis.CCI)
                    GenerarPaginaCci(analisis);
                else
                    GenerarPaginaFusibilidad(analisis);
            };

            PageAnalisisEquipo.BorrarAnalisisCci += (analisis)=>{
                BorrarAnalisisCci(analisis);
            };

            PageAnalisisEquipo.RecargarAnalisis += () =>
            {
                return GetAnalisis();
            };

            PageAnalisisEquipo.GuardarOrden += (analisis) =>
            {
                FusibilidadControl control = analisis.ConversorControlFus();
                control.Update(null, "OrdenEnsayo");
            };
        }

        private void BorrarAnalisisCci(Analisis analisis)
        {
            using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
            using (NpgsqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    FusibilidadControl control = analisis.ConversorControlFus();
                    control.Replica = PersistenceManager.SelectByProperty<ReplicaFusibilidadControl>("IdFusibilidad", control.Id).FirstOrDefault();
                    control.Replica.Delete(conn);
                    control.Delete(conn);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    if (trans != null)
                        trans.Rollback();

                    CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "Error al borrar el análisis", ex);
                    MessageBox.Show("Error al borrar el análisis. Por favor, informa a soporte.");
                }
            }
        }

        private void GenerarPaginaCci(Analisis analisis)
        {
            FusibilidadControl control;
            if (analisis == null)
            {
                control = FactoriaFusibilidadControl.GetDefault(Ensayo.Id);
                control.OrdenEnsayo = PageAnalisisEquipo.ListaAnalisis.Select(a => a.Orden).DefaultIfEmpty(0).Max() + 1;
            }
            else
            {
                control = analisis.ConversorControlFus();
                control.Load();
                control.Replica = PersistenceManager.SelectByProperty<ReplicaFusibilidadControl>("IdFusibilidad", control.Id).FirstOrDefault();
            }

            CargarCci(control);
        }

        private void GenerarPaginaFusibilidad(Analisis analisis)
        {
            Fusibilidad fus = FactoriaFusibilidad.GetParametro(analisis.IdMuestra) ?? FactoriaFusibilidad.GetDefault(analisis.IdMuestra);
            CargarFusibilidad(fus);
        }

        private void CargarCci(FusibilidadControl control)
        {
            stackEnsayo.Visibility = Visibility.Collapsed;
            ControlFusCci c = new ControlFusCci(Ensayo.Id) {FusibilidadControl=control };
            stack.Children.Add(c);

            c.BackButtonClick += (s, e) =>
            {
                Volver(c);
            };
        }

        private void CargarFusibilidad(Fusibilidad fus)
        {
            stackEnsayo.Visibility = Visibility.Collapsed;
            ControlFusAnalisis c = new ControlFusAnalisis() { Fusibilidad = fus };
            stack.Children.Add(c);

            c.BackButtonClick += (s, e) =>
            {
                Volver(c);
            };
        }

        private void Volver(UserControl c)
        {
            stackEnsayo.Visibility = Visibility.Visible;
            stack.Children.Remove(c);

            PageAnalisisEquipo.RecargarPagina();
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
