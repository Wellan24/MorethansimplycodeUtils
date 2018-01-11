using Cartif.Util;
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
    /// Lógica de interacción para PropertyControlCheckBox.xaml
    /// </summary>
    public partial class PropertyControlCheckBox : PropertyControl
    {

        public PropertyControlCheckBox()
        {
            InitializeComponent();
        }

        public override bool IsValid => true;

        public override string Label
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
                    this.label.Content = value;
            }
        }

        public override bool? IsCheceked
        {
            get{ return this.innerContent.IsChecked; }

            set
            {
                if (value != null)
                    this.innerContent.IsChecked = value ?? false;
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
            get { return !this.innerContent.IsEnabled; }
            set
            {
                if (value != null)
                    this.innerContent.IsEnabled = !value ?? true;
            }
        }

        public override String ControlToolTipText
        {
            get { return this.innerContent.ToolTip.ToString(); }
            set { this.innerContent.ToolTip = value; }
        }

        public override void AddCheckedChanged(RoutedEventHandler handler)
        {
            if (handler != null)
            {
                innerContent.Checked += handler;
                innerContent.Unchecked += handler;
            }
        }

        public override void SetContentBinding(Object obj)
        {
            BindUtils.Bind(obj, PropertyName, this.innerContent, CheckBox.IsCheckedProperty, new GenericValidation<PropertyControl>(this, Validate, OnValid, OnInvalid));
        }

        public override HorizontalAlignment? HorizontalAlignment
        {
            get {return innerContent.HorizontalAlignment; }

            set
            {
                if (value != null)
                    innerContent.HorizontalAlignment = value ?? System.Windows.HorizontalAlignment.Left;
            }
        }

    }
}
