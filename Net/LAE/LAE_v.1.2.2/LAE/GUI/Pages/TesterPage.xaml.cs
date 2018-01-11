using Cartif.Extensions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using GUI.Windows;
using LAE.DocWord;
using LAE.Modelo;
using Persistence;
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

            Version myVersion = Assembly.GetExecutingAssembly()?.GetName()?.Version; //no muestra versión correcta

            //Version myVersion = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion; //necesita añadir referencia System.Deployment, pero da error al ejecutar

            if (myVersion != null)
                labelVersion.Content = String.Format("Gestion LAE v{0}.{1}.{2}.{3}", myVersion.Build, myVersion.Revision, myVersion.Major, myVersion.Minor);

            CrearParam();

        }

        private void CrearParam()
        {
            Procedimiento[] lista = PersistenceManager.SelectAll<Procedimiento>().ToArray();
            foreach (Procedimiento proc in lista)
            {
                System.Windows.Controls.CheckBox cb = new System.Windows.Controls.CheckBox();
                cb.Content = proc.Siglas;
                cb.Tag = proc.Id;
                cb.Width = 70;
                cb.Height = 20;
                //listaParam.Children.Add(cb);
                
                listaParam2.Children.Add(cb);
            }

            ParametroMuestraBiomasa p = new ParametroMuestraBiomasa() { IdMuestra = 1, IdProcedimiento = 2 };
            ParametroMuestraBiomasa p2 = new ParametroMuestraBiomasa() { IdMuestra = 1, IdProcedimiento = 3 };
            ParametroMuestraBiomasa[] pmb = new ParametroMuestraBiomasa[] { p, p2 };
            listaParam2.Children.OfType<System.Windows.Controls.CheckBox>().ForEach(ch => {
                if (pmb.Any(pr => pr.IdProcedimiento == (int)ch.Tag)){
                    ch.IsChecked = true;
                }
            });

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
    }
}
