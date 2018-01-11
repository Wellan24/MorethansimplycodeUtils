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
    /// Lógica de interacción para PropertyControlButton.xaml
    /// </summary>
    public partial class PropertyControlButton : PropertyControl
    {
        public PropertyControlButton()
        {
            InitializeComponent();
        }
        public override Boolean IsValid
        {
            get { return true; }
        }

        public override bool? ReadOnly
        {
            get { return !this.btn.IsEnabled; }

            set
            {
                if (value != null)
                    this.btn.IsEnabled = !value ?? false;
            }
        }

        public override string DesignPath
        {
            set { innerPath.Data = Geometry.Parse(value); }
        }


        public override Brush BackgroundColor
        {
            set {
                if (value != null)
                    btn.Background = value;
            }
        }

        public override Action<object, RoutedEventArgs> Click
        {
            set
            {
                if (value != null)
                    btn.AddHandler(Button.ClickEvent, new RoutedEventHandler(value));
            }
        }

    }
}
