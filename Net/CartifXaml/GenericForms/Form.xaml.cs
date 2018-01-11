using GenericForms.Abstract;
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
using System.Windows.Shapes;

namespace GenericForms
{
    /// <summary>
    /// Lógica de interacción para Form.xaml
    /// </summary>
    public partial class Form : Window
    {
        private Dictionary<String, IPropertyControlSettings> innerFields;
        public Dictionary<String, IPropertyControlSettings> InnerFields
        {
            get { return innerFields; }
            set { innerFields = value; }
        }

        private Object innerValue;
        public Object InnerValue
        {
            get { return innerValue; }
            set { innerValue = value; }
        }

        public Form()
        {
            InitializeComponent();
        }

        public void Build(Dictionary<string, IPropertyControlSettings> innerFields, Object innerValue)
        {
            this.InnerFields = innerFields;
            this.InnerValue = InnerValue;
            Build();
        }

        public void Build()
        {
            if (innerFields != null)
            {
                foreach (var item in innerFields)
                {
                    PropertyControl pc = FactoryPropertyControl.Build(InnerValue, item.Key, item.Value, null);
                    root.Children.Add(pc);
                }
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Contacto c = InnerValue as Contacto;
            MessageBox.Show(c.IdCliente.ToString());
        }
    }
}
