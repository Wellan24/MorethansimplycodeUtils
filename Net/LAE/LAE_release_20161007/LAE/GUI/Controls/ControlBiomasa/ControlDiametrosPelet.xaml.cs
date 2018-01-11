using LAE.Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace GUI.Controls
{
    /// <summary>
    /// Lógica de interacción para ControlDiametrosPelet.xaml
    /// </summary>
    public partial class ControlDiametrosPelet : UserControl
    {

        private ClasePelet clase;
        public ClasePelet Clase
        {
            get { return clase; }
            set
            {
                clase = value;
                GenerarDiametros();
            }
        }

        public Action UpdateData { get; set; }
        
        public ControlDiametrosPelet()
        {
            InitializeComponent();
        }
        

        private void GenerarDiametros()
        {
            int numeroDiametros = 10; /* nº diametros por defecto */
            int l = Clase.Diametros.Count;
            int idMilimetros = Unidad.Of("Milimetros").Id;
            /* numerar diámetros */
            for (int i = 0; i < numeroDiametros; i++)
            {
                if (i < l)
                {
                    Clase.Diametros[i].Numero = (i + 1);
                }
                else
                {
                    Clase.Diametros.Add(new DiametroPelet() { Numero = i + 1, IdUdsMedida= idMilimetros });
                }
            }
            patron.ItemsSource = Clase.Diametros;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateData();
        }

    }
}
