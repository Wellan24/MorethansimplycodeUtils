using Cartif.Extensions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using GUI.Windows;
using LAE.DocWord;
using LAE.Modelo;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
using LAE.Biomasa.Ventanas;
using LAE.Biomasa.Pages;

namespace GUI.Pages
{
    /// <summary>
    /// Lógica de interacción para TesterPage.xaml
    /// </summary>
    public partial class TesterPage : UserControl
    {
        public TesterPage()
        {
            InitializeComponent();

            //var a = this.Resources["HighlightColor"];
            //var b = this.Resources["TextHighlightBrush"];
            //var c = this.Resources["MetroWindowCloseButtonStyle"];
            //var d = Application.Current.Resources["HighlightColor"];
            //var e = Application.Current.Resources["TextHighlightBrush"];
            //var f = Application.Current.Resources["MetroWindowCloseButtonStyle"];

            

            //SolidColorBrush sbr = (SolidColorBrush)Application.Current.Resources["HighlightColor"];
            //Brush br=(Brush)Application.Current.Resources["HighlightColor"];

            Version myVersion = Assembly.GetExecutingAssembly()?.GetName()?.Version; //no muestra versión correcta

            //Version myVersion = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion; //necesita añadir referencia System.Deployment, pero da error al ejecutar

            if (myVersion != null)
                labelVersion.Content = String.Format("Gestion LAE v{0}.{1}.{2}.{3}", myVersion.Build, myVersion.Revision, myVersion.Major, myVersion.Minor);
        }       

        private void CrearParam()
        {
        }

        private void expand_Click(object sender, RoutedEventArgs e)
        {

            PlanesMedicion ventanaPlanMedicion = new PlanesMedicion(1, 2, new PlanMedicionAtmosfera());

            ventanaPlanMedicion.ShowDialog();
        }

        private void soporte_Click(object sender, RoutedEventArgs e)
        {
            Soportes ventanaSoporte = new Soportes();
            ventanaSoporte.ShowDialog();
        }

        private void biomasa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PNTsBiomasa ventana = new PNTsBiomasa(1);
                ventana.ShowDialog();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void maquinaCHN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WindowEquipoCHN ventana = new WindowEquipoCHN();
                ventana.ShowDialog();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
