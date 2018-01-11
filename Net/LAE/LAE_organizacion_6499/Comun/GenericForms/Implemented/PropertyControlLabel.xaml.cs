using GenericForms.Abstract;
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

namespace GenericForms.Implemented
{
    /// <summary>
    /// Lógica de interacción para PropertyControlLabel.xaml
    /// </summary>
    public partial class PropertyControlLabel : PropertyControl
    {
        public PropertyControlLabel()
        {
            InitializeComponent();
        }

        public override string Label
        {
            get { return label.Content.ToString(); }

            set
            {
                label.Content = value;
            }
        }
    }
}
