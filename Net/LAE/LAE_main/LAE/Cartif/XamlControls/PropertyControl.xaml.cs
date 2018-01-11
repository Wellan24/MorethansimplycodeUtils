using LAE.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LAE
{
    public partial class PropertyControl : UserControl
    {
        public Func<Object, Boolean> Validar { get; set; }
        public Control Control { get; set; }

        public PropertyControl()
        {
            this.InitializeComponent();
        }
    }
}