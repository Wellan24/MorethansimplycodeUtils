using LAE.Biomasa.Modelo;
using MahApps.Metro.Controls;
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
using LAE.Comun.Modelo.Procedimientos;
using LAE.Biomasa.Controles;
using LAE.Biomasa.Pages;
using Npgsql;
using Cartif.Logs;

namespace LAE.Biomasa.Ventanas
{
    /// <summary>
    /// Lógica de interacción para WindowEquipoCHN.xaml
    /// </summary>
    public partial class WindowEquipoCHN : MetroWindow
    {
        public static readonly DependencyProperty IconTitleProperty =
               DependencyProperty.Register("IconTitle", typeof(ImageSource), typeof(WindowEquipoCHN), new PropertyMetadata(null));
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
                    tabAnalisis.Visibility = Visibility.Visible;
                PageDerivaEquipoCHN.CHNderiva = Ensayo.Id == 0 ? FactoriaChnDeriva.GetDefault(Ensayo.Id) : (FactoriaChnDeriva.GetCHNderiva(Ensayo.Id) ?? FactoriaChnDeriva.GetDefault(Ensayo.Id));
                PageDerivaEquipoCHN.Ensayo = Ensayo;
                EquipoChnEventos();

                /*Pagina analisis*/
                MuestraAnalisis[] muestras = GetMuestrasAnalisis();
                Analisis[] analisis = GetAnalisis();
                PageAnalisisEquipo.SetDataPage(muestras, analisis, Ensayo);
            }
        }

        public WindowEquipoCHN()
        {
            InitializeComponent();
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE.Comun;component/images/cabecera.png", UriKind.Absolute));
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        private MuestraAnalisis[] GetMuestrasAnalisis()
        {
            MuestraRecepcionBiomasa[] muestras = FactoriaMuestraRecepcionBiomasa.GetMuestrasEnsayoChn(Ensayo.Id);

            /*Obtengo todos materiales, porque son muy pocos y no hacen la query lenta*/
            ChnMaterialReferencia[] materiales = PersistenceManager.SelectAll<ChnMaterialReferencia>().ToArray();

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
            ChnControl[] controlesCalidad = PersistenceManager.SelectByProperty<ChnControl>("IdEnsayo", Ensayo.Id).ToArray();

            List<Analisis> lista = muestrasEnsayo.Select(l => new Analisis()
            {
                Id = l.Id,
                IdMuestra = l.IdMuestra,
                Orden = l.OrdenEnsayo,
                IdHumedad = l.IdHumedad,
                IdHumedad3 = l.IdHumedad3,
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

        private void EquipoChnEventos()
        {

            PageDerivaEquipoCHN.VisibilidadAnalisis += (s, e) =>
            {
                if (Ensayo.Id != 0)
                    tabAnalisis.Visibility = Visibility.Visible;
            };

            PageAnalisisEquipo.VerAnalisis += (s, e) =>
            {
                Analisis analisis = PageAnalisisEquipo.gridAnalisis.DataGrid.SelectedItem as Analisis;
                if (analisis == null || analisis.CCI)
                    GenerarPaginaCci(analisis);
                else
                    GenerarPaginaChn(analisis);
            };

            PageAnalisisEquipo.BorrarAnalisisCci += (analisis) =>
            {
                BorrarAnalisisCci(analisis);
            };

            PageAnalisisEquipo.RecargarAnalisis += () =>
            {
                return GetAnalisis();
            };

            PageAnalisisEquipo.GuardarOrden += (analisis) =>
            {
                ChnControl control = analisis.ConversorControlChn();
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
                    ChnControl control = analisis.ConversorControlChn();
                    control.Replicas = PersistenceManager.SelectByProperty<ReplicaChnControl>("IdCHN", control.Id).ToList();
                    PersistenceDataManipulation.Borrar(conn, control.Replicas);
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
            ChnControl control;
            if (analisis == null)
            {
                control = FactoriaCHNControl.GetDefault(Ensayo.Id);
                control.OrdenEnsayo = PageAnalisisEquipo.ListaAnalisis.Select(a => a.Orden).DefaultIfEmpty(0).Max() + 1;
            }
            else
            {
                control = analisis.ConversorControlChn();
                control.Load();
                control.Replicas = PersistenceManager.SelectByProperty<ReplicaChnControl>("IdCHN", control.Id).ToList();
            }

            CargarCci(control);
        }

        private void GenerarPaginaChn(Analisis analisis)
        {
            Chn chn = FactoriaCHN.GetParametro(analisis.IdMuestra) ?? FactoriaCHN.GetDefault(analisis.IdMuestra);
            CargarChn(chn);
        }

        private void CargarCci(ChnControl control)
        {
            TabControl.Visibility = Visibility.Collapsed;
            ControlCHNcci c = new ControlCHNcci(Ensayo.Id) { CHNcontrol = control };
            stack.Children.Add(c);

            c.BackButtonClick += (s, e) =>
            {
                Volver(c);
            };
        }

        private void CargarChn(Chn chn)
        {
            TabControl.Visibility = Visibility.Collapsed;
            ControlChnAnalisis c = new ControlChnAnalisis() { Chn = chn };
            stack.Children.Add(c);

            c.BackButtonClick += (s, e) =>
            {
                Volver(c);
            };

        }

        private void Volver(UserControl c)
        {
            TabControl.Visibility = Visibility.Visible;
            stack.Children.Remove(c);

            PageAnalisisEquipo.RecargarPagina();
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
