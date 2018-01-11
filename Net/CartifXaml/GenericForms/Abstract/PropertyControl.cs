using Cartif.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace GenericForms.Abstract
{
    public class PropertyControl : UserControl
    {

        public virtual Object[] InnerValues { get; set; }
        public virtual String PathValue { get; set; }
        public virtual String PropertyName { get; set; }
        public virtual String Label { get; set; }
        public virtual String ControlToolTipText { get; set; }

        public virtual void SetContentBinding(Object obj) { }
        public virtual void ShowMessage(String mensaje, Brush color) { }
        public virtual void HideMessage() { }

        public virtual Func<Object, Boolean> Validate { get; set; }
        public virtual Action<PropertyControl> OnValid { get; set; }
        public virtual Action<PropertyControl> OnInvalid { get; set; }
        public virtual Type Type { get; set; }

        public virtual Boolean IsValid { get; }

    }
}
