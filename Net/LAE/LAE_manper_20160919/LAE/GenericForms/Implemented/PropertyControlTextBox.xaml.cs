using Cartif.Util;
using GenericForms.Abstract;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Lógica de interacción para PropertyControlTextBox.xaml
    /// </summary>
    public partial class PropertyControlTextBox : PropertyControl
    {
        public PropertyControlTextBox()
        {
            InitializeComponent();
        }

        public override Boolean IsValid
        {
            get { return (Validate?.Invoke(this.innerContent.Text) ?? true) && Validation.GetErrors(innerContent).Count == 0; }
        }

        public override String Label
        {
            get { return this.label.Content.ToString(); }
            set
            {
                if ("".Equals(value))
                {
                    FirstColumn.Width = new GridLength(0);
                    FirstColumn.MinWidth = 0;
                }
                else
                    this.label.Content = value + ":";
            }
        }



        public override Boolean? Enabled
        {
            get { return this.innerContent.IsEnabled; }
            set
            {
                if (value != null)
                    this.innerContent.IsEnabled = value ?? true;
            }
        }

        public override Boolean? ReadOnly
        {
            get { return this.innerContent.IsReadOnly; }
            set
            {
                if (value != null)
                    this.innerContent.IsReadOnly = value ?? false;
            }
        }
        public override Brush BackgroundColor
        {
            set
            {
                if (value != null)
                    this.innerContent.Background = value;
            }
        }

        public override String ControlToolTipText
        {
            get { return this.innerContent.ToolTip.ToString(); }
            set { this.innerContent.ToolTip = value; }
        }

        public override double HeightMultiline
        {
            get { return panel.Height; }
            set
            {
                if (value != 0)
                {
                    innerContent.TextWrapping = TextWrapping.Wrap;
                    innerContent.AcceptsReturn = true;
                    panel.Height = value;
                }
            }
        }

        public override void AddTextChanged(TextChangedEventHandler handler)
        {
            if (handler != null)
                innerContent.TextChanged += handler;
        }

        public override void AddKeyDown(KeyEventHandler handler)
        {
            if (handler != null)
                innerContent.KeyDown += handler;
        }


        public override void SetContentBinding(Object obj)
        {
            BindUtils.Bind(obj, PropertyName, this.innerContent, TextBox.TextProperty, new GenericValidation<PropertyControl>(this, Validate, OnValid, OnInvalid));
        }

        public override void SetContentBinding(Object obj, Object targetNull)
        {
            BindUtils.Bind(obj, PropertyName, this.innerContent, TextBox.TextProperty, new GenericValidation<PropertyControl>(this, Validate, OnValid, OnInvalid), targetNull);
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


        public override void SetInnerContent(Object o)
        {
            try {
                if (o is decimal)
                    this.innerContent.Text = ((decimal)o).ToString(CultureInfo.InvariantCulture);
                else if (o is double)
                    this.innerContent.Text = ((double)o).ToString(CultureInfo.InvariantCulture);
                else
                    this.innerContent.Text = o?.ToString();
            }
            catch (Exception)
            {
                this.innerContent.Text = o?.ToString();
            }
        }
    }
}
