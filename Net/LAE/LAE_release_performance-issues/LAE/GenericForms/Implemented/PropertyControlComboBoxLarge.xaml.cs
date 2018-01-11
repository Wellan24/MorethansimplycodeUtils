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
using Cartif.Extensions;

namespace GenericForms.Implemented
{
    /// <summary>
    /// Lógica de interacción para PropertyControlComboBoxLarge.xaml
    /// </summary>
    public partial class PropertyControlComboBoxLarge : PropertyControl
    {
        public PropertyControlComboBoxLarge()
        {
            InitializeComponent();
        }

        //public override Boolean IsValid => Validate?.Invoke(this.innerContent.Text) ?? true;
        public override Boolean IsValid => Validate?.Invoke(this.innerContent.SelectedItem) ?? true;

        public override Object[] InnerValues
        {
            get { return innerContent.Items.OfType<Object>().ToArray(); }
            set
            {
                innerContent.Items.Clear();
                value?.ForEach(i => innerContent.Items.Add(i));
                innerContent.SelectedItem = null;
            }
        }

        public override string DisplayMemberPath
        {
            get { return innerContent.DisplayMemberPath; }
            set { if (value != null) innerContent.DisplayMemberPath = value; }
        }

        public override String Label
        {
            get { return this.label.Content.ToString(); }
            set
            {
                if ("".Equals(value))
                {
                    this.label.Content = String.Empty;
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
            get { return !this.innerContent.IsEnabled; }
            set
            {
                if (value != null)
                    this.innerContent.IsEnabled = !value ?? true;
            }
        }

        public override String ControlToolTipText
        {
            get { return this.innerContent.ToolTip?.ToString(); }
            set { this.innerContent.ToolTip = value; }
        }

        public override String PathValue
        {
            get { return innerContent.SelectedValuePath ?? "Id"; }
            set { innerContent.SelectedValuePath = value ?? "Id"; }
        }

        public override void AddSelectionChanged(SelectionChangedEventHandler handler)
        {
            if (handler != null)
                innerContent.SelectionChanged += handler;
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

        public override Action<object, KeyEventArgs> KeyDownCombo
        {
            set
            {
                if (value != null)
                    innerContent.AddHandler(ComboBox.KeyDownEvent, new KeyEventHandler(value));
            }
        }
    }
}
