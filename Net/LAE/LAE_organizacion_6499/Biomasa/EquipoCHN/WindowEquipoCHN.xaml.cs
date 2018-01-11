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

namespace LAE.Biomasa.Pages
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
                PageAnalisisEquipoCHN.Ensayo = Ensayo;
                EquipoCHNcci();
            }
        }

        public WindowEquipoCHN()
        {
            InitializeComponent();
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE;component/images/cabecera.png", UriKind.Absolute));
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        private void EquipoCHNcci()
        {
            PageAnalisisEquipoCHN.VerCCI += (s, e) =>
            {
                AnalisisCHN analisis = PageAnalisisEquipoCHN.gridAnalisis.DataGrid.SelectedItem as AnalisisCHN;
                ChnControl control;
                if (analisis == null)
                {
                    control = FactoriaCHNControl.GetDefault(Ensayo.Id);
                    control.OrdenEnsayo = PageAnalisisEquipoCHN.ListaAnalisis.Select(a => a.Orden).DefaultIfEmpty(0).Max() + 1;
                }
                else
                {
                    control = analisis.ConversorControl();
                    control.Load();
                    control.Replicas = PersistenceManager.SelectByProperty<ReplicaChnControl>("IdCHN", control.Id).ToList();
                }

                CargarCCI(control);

            };

            PageDerivaEquipoCHN.VerAnalisis += (s, e) =>
            {
                if (Ensayo.Id != 0)
                    tabAnalisis.Visibility = Visibility.Visible;
            };
        }

        private void CargarCCI(ChnControl control)
        {
            TabControl.Visibility = Visibility.Collapsed;
            ControlCHNcci c = new ControlCHNcci(Ensayo.Id) { CHNcontrol = control };
            stack.Children.Add(c);

            c.BackButtonClick += (s, e) =>
            {
                Volver(c);
            };
        }

        private void Volver(ControlCHNcci c)
        {
            TabControl.Visibility = Visibility.Visible;
            stack.Children.Remove(c);

            PageAnalisisEquipoCHN.RecargarPagina();
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
