using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using LAE.DocWord;
using LAE.Modelo;
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
            CargarDatos();
            CargarDatos2();
        }

        private void CargarDatos2()
        {
            List<Persona> names2 = new List<Persona>();
            names2.Add(new Persona("Juan", "Perez"));
            names2.Add(new Persona("Luis", "Gomez"));
            names2.Add(new Persona("Lucia", "Sanz"));
            comboFilter2.ItemsSource = names2;
            comboFilter2.IsTextSearchCaseSensitive = false;
        }

        private void CargarDatos()
        {
            List<String> names = new List<string>();
            names.Add("WPF rocks");
            names.Add("WCF rocks");
            names.Add("XAML is fun");
            names.Add("WPF rules");
            names.Add("WCF rules");
            names.Add("WinForms not");
            comboFilter.ItemsSource = names;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            RevisionOferta r = PersistenceManager.SelectAll<RevisionOferta>().FirstOrDefault();
            //new Escritor("word/O-LAE.docx", new DocOfertas(r));
            new Escritor("word/O-LAE.docx","", new DocOfertas(r));
        }

        private void comboFilter2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                MessageBox.Show("enter");
            MessageBox.Show("Prueba");
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {

        }
    }

    public class Persona
    {
        public String Nombre { get; set; }
        public String Apellidos { get; set; }
        public Persona(String n, String a)
        {
            Nombre = n;
            Apellidos = a;
        }
    }
}
