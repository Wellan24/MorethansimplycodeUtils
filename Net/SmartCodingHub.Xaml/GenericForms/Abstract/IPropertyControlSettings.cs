using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GenericForms.Abstract
{
    public interface IPropertyControlSettings
    {
        Object[] InnerValues { get; set; }
        IPropertyControlSettings SetInnerValues(Object[] newInnerValues);

        String PathValue { get; set; }
        IPropertyControlSettings SetPathValue(String newPathValue);

        String Label { get; set; }
        IPropertyControlSettings SetLabel(String newLabel);

        String ControlToolTipText { get; set; }
        IPropertyControlSettings SetControlToolTipText(String newControlToolTipText);

        Func<Object, Boolean> Validate { get; set; }
        IPropertyControlSettings SetValidate(Func<Object, Boolean> newValidate);

        Action<PropertyControl> OnValid { get; set; }
        IPropertyControlSettings SetOnValid(Action<PropertyControl> newOnValid);

        Action<PropertyControl> OnInvalid { get; set; }
        IPropertyControlSettings SetOnInvalid(Action<PropertyControl> newOnInvalid);

        Type Type { get; set; }
        IPropertyControlSettings SetType(Type newType);

    }
}
