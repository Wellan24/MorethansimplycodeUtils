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
    public interface IPropertyControlSettings
    {
        Object[] InnerValues { get; set; }
        IPropertyControlSettings SetInnerValues(Object[] newInnerValues);

        String DisplayMemberPath { get; set; }
        IPropertyControlSettings SetDisplayMemberPath(String newDisplayMemberPath);

        String PathValue { get; set; }
        IPropertyControlSettings SetPathValue(String newPathValue);

        String Label { get; set; }
        IPropertyControlSettings SetLabel(String newLabel);

        double? MinWidthLabel { get; set; }
        IPropertyControlSettings SetMinWidthLabel(double? newMinWidthLabel);

        Boolean? Enabled { get; set; }
        IPropertyControlSettings SetEnabled(Boolean newEnabled);

        Boolean? ReadOnly { get; set; }
        IPropertyControlSettings SetReadOnly(Boolean newReadOnly);

        String ControlToolTipText { get; set; }
        IPropertyControlSettings SetControlToolTipText(String newControlToolTipText);

        double HeightMultiline { get; set; }
        IPropertyControlSettings SetHeightMultiline(double newHeightMultiline);

        //int? SelectedIndex { get; set; }
        //IPropertyControlSettings SetSelectedIndex(int? newSelectedIndex);

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

        RoutedPropertyChangedEventHandler<object> ValueChanged { get; }
        IPropertyControlSettings AddValueChanged(RoutedPropertyChangedEventHandler<object> newValueChanged);

        RoutedEventHandler CheckedChanged { get;}
        IPropertyControlSettings AddCheckedChanged(RoutedEventHandler newCheckedChanged);

        TextChangedEventHandler TextChanged { get; }
        IPropertyControlSettings AddTextChanged(TextChangedEventHandler newTextChanged);

        KeyEventHandler KeyDown { get; }
        IPropertyControlSettings AddKeyDown(KeyEventHandler newKeyDown);

        String DesignPath { get; set; }
        IPropertyControlSettings SetDesignPath(String newDesignPath);

        Action<object, RoutedEventArgs> Click { get; set; }
        IPropertyControlSettings AddClick(Action<object, RoutedEventArgs> newClick);

        Action<object, KeyEventArgs> KeyDownCombo { get; set; }
        IPropertyControlSettings AddKeyDownCombo(Action<object, KeyEventArgs> newKeyDown);

        Brush BackgroundColor { get; set; }
        IPropertyControlSettings SetColor(Brush newBackgroundColor);

        HorizontalAlignment? HorizontalAlignment { get; set; }
        IPropertyControlSettings SetHorizontalAlignment(HorizontalAlignment newHorizontalAlignment);

        Object TargetNull { get; set; }
        IPropertyControlSettings SetTargetNull(Object newTargetNull);

    }
}
