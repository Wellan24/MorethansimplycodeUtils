using Cartif.Util;
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
using Cartif.Extensions;
using GenericForms.Abstract;

namespace GenericForms.Implemented
{
    /// <summary>
    /// Lógica de interacción para PropertyControlComboBox.xaml
    /// </summary>
    public partial class PropertyControlComboBox : PropertyControl
    {
        public PropertyControlComboBox()
        {
            InitializeComponent();
        }

        public override Boolean IsValid => Validate?.Invoke(this.innerContent.Text) ?? true;

        public override Object[] InnerValues
        {
            get { return innerContent.Items.OfType<Object>().ToArray(); }
            set
            {
                innerContent.Items.Clear();
                value?.ForEach(i => innerContent.Items.Add(i));
            }
        }

        public override String Label
        {
            get { return this.label.Content.ToString(); }
            set { this.label.Content = value; }
        }

        public override String ControlToolTipText
        {
            get { return this.innerContent.ToolTip.ToString(); }
            set { this.innerContent.ToolTip = value; }
        }

        public override String PathValue
        {
            get { return innerContent.SelectedValuePath ?? "Id"; }
            set { innerContent.SelectedValuePath = value ?? "Id"; }
        }

        public override void SetContentBinding(Object obj)
        {
            BindUtils.Bind(obj, PropertyName, this.innerContent, ComboBox.SelectedValueProperty, new GenericValidation<PropertyControl>(this, Validate, OnValid, OnInvalid));
        }

        override public void ShowMessage(String mensaje, Brush color)
        {
            this.mensaje.Content = mensaje;
            this.mensaje.Foreground = color;
            this.mensaje.Visibility = Visibility.Visible;
        }

        override public void HideMessage()
        {
            this.mensaje.Visibility = Visibility.Collapsed;
        }
    }
}
