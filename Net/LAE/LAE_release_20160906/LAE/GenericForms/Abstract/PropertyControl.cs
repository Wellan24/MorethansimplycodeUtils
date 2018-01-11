using Cartif.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GenericForms.Abstract
{
    public class PropertyControl : UserControl
    {

        public virtual Object[] InnerValues { get; set; }
        public virtual String DisplayMemberPath { get; set; }
        public virtual String PathValue { get; set; }
        public virtual String PropertyName { get; set; }
        public virtual String Label { get; set; }
        public virtual Boolean? Enabled { get; set; }
        public virtual Boolean? ReadOnly { get; set; }
        public virtual String ControlToolTipText { get; set; }
        public virtual double HeightMultiline { get; set; }
        public virtual int? SelectedIndex { get; set; } /* combobox - no para factoria, ya que se usa una vez construido */ /* solo cambia el valor si no tine uno asignado previamente */

        public virtual int ColumnSpan { get; set; }

        public virtual void SetContentBinding(Object obj) { }
        public virtual void SetContentBinding(Object obj, Object targetNull) { }
        public virtual void ShowMessage(String mensaje, Brush color) { }
        public virtual void HideMessage() { }
        public virtual void SelectionText(int start, int end) { }

        public virtual Func<Object, Boolean> Validate { get; set; }
        public virtual Action<PropertyControl> OnValid { get; set; }
        public virtual Action<PropertyControl> OnInvalid { get; set; }
        public virtual Type Type { get; set; }

        public virtual Boolean IsValid { get; }

        public virtual void AddSelectionChanged(SelectionChangedEventHandler handler) { } /* combobox */
        public virtual void AddValueChanged(RoutedPropertyChangedEventHandler<object> handler) { } /* datepicker */
        public virtual void AddCheckedChanged(RoutedEventHandler handler){ } /* checkbox */
        public virtual void AddTextChanged(TextChangedEventHandler handler) { }/* textbox */
        public virtual void AddKeyDown(KeyEventHandler handler) { } /* textbox */

        public virtual void SetInnerContent(Object o) { }

        /* for PropertyControlButton */
        public virtual String DesignPath { get; set; }
        public virtual Action<object, RoutedEventArgs> Click { get; set; }
        public virtual Action<object, KeyEventArgs> KeyDownCombo { get; set; }
        public virtual Brush BackgroundColor { get; set; }
        public virtual HorizontalAlignment? HorizontalAlignment { get; set; }

        public void ChangeVisibiliy(bool isPCVisible)
        {
            this.Visibility = isPCVisible ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
