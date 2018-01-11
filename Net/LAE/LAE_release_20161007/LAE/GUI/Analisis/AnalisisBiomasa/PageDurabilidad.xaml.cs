using LAE.Calculos;
using LAE.Modelo;
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

namespace GUI.Analisis
{
    /// <summary>
    /// Lógica de interacción para PageDurabilidad.xaml
    /// </summary>
    public partial class PageDurabilidad : UserControl
    {

        private MedicionPNT medicion;
        public MedicionPNT Medicion
        {
            get { return medicion; }
            set
            {
                medicion = value;
                Prueba.Medicion = medicion;
                AddCCI(medicion);
            }
        }

        public PageDurabilidad()
        {
            InitializeComponent();
            Prueba.CalculoFinos = RealizarCalculoFinos;
            CCI.CalculoFinos = RealizarCalculoFinos;

            Prueba.CalculoDurabilidad = RealizarCalculoDurabilidad;
            CCI.CalculoDurabilidad = RealizarCalculoDurabilidad;
        }

        private void AddCCI(MedicionPNT med)
        {
            MedicionPNT medCCI = FactoriaMedicionPNTcci.GetMedicion(med.Id);
            if (medCCI != null)
            {
                CCI.Medicion = medCCI;
                CCI.Visibility = Visibility.Visible;
                CCIAceptacion.Visibility = Visibility.Visible;
            }
            else
                CCI.Medicion = FactoriaMedicionPNT.GetDefault(Medicion.IdTecnico, Medicion.IdMuestra);
        }

        private void AddDeleteCCI_Click(object sender, RoutedEventArgs e)
        {
            if (CCI.Visibility == Visibility.Visible)
            {
                CCI.Visibility = Visibility.Collapsed;
                CCIAceptacion.Visibility = Visibility.Collapsed;
            }
            else {
                CCI.Visibility = Visibility.Visible;
                CCIAceptacion.Visibility = Visibility.Visible;
            }
        }

        private void RealizarCalculoFinos()
        {
            Finos finos = new Finos();
            if (Prueba.Finos?.MediaFinos != null && CCI.Finos?.MediaFinos != null)
            {
                finos.IdVProcedimiento = Prueba.Finos.IdVProcedimiento;

                Valor[] valoresFinos = new Valor[] { Valor.Of(Prueba.Finos.MediaFinos, "%"), Valor.Of(CCI.Finos.MediaFinos, "%") };

                finos.MediaFinos = Calcular.Promedio(valoresFinos).Value;

                CCIAceptacion.Finos = finos;
            }
            else
                CCIAceptacion.ClearFinos();

        }

        private void RealizarCalculoDurabilidad()
        {
            Durabilidad durabilidad = new Durabilidad();
            if (Prueba.Durabilidad?.MediaDurabilidad != null && CCI.Durabilidad?.MediaDurabilidad != null)
            {
                durabilidad.IdVProcedimiento = Prueba.Durabilidad.IdVProcedimiento;

                Valor[] valoresDurabilidad = new Valor[] { Valor.Of(Prueba.Durabilidad.MediaDurabilidad, "%"), Valor.Of(CCI.Durabilidad.MediaDurabilidad, "%") };

                durabilidad.MediaDurabilidad = Calcular.Promedio(valoresDurabilidad).Value;
                durabilidad.Dif = Calcular.DiferenciaAbsoluta(valoresDurabilidad).Value;

                CCIAceptacion.Durabilidad = durabilidad;
            }
            else
                CCIAceptacion.ClearDurabilidad();
        }
    }
}
