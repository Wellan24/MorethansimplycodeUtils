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

        Boolean? Enabled { get; set; }
        IPropertyControlSettings SetEnabled(Boolean newEnabled);

        Boolean? ReadOnly { get; set; }
        IPropertyControlSettings SetReadOnly(Boolean newReadOnly);

        String ControlToolTipText { get; set; }
        IPropertyControlSettings SetControlToolTipText(String newControlToolTipText);

        double HeightMultiline { get; set; }
        IPropertyControlSettings SetHeightMultiline(double newHeightMultiline);

        int ColumnSpan { get; set; }
        IPropertyControlSettings SetColumnSpan(int newColumnSpan);

        Func<Object, Boolean> Validate { get; set; }
        IPropertyControlSettings SetValidate(Func<Object, Boolean> newValidate);

        Action<PropertyControl> OnValid { get; set; }
        IPropertyControlSettings SetOnValid(Action<PropertyControl> newOnValid);

        Action<PropertyControl> OnInvalid { get; set; }
        IPropertyControlSettings SetOnInvalid(Action<PropertyControl> newOnInvalid);

        Type Type { get; set; }
        IPropertyControlSettings SetType(Type newType);

        SelectionChangedEventHandler SelectionChanged { get; }
        IPropertyControlSettings AddSelectionChanged(SelectionChangedEventHandler newSelectionChanged);


    }
}
