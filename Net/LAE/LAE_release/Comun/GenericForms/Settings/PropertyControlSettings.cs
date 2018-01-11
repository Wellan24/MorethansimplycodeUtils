using GenericForms.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GenericForms.Settings
{
    public class PropertyControlSettings : IPropertyControlSettings
    {
        public static PropertyControlSettings Empty { get; } = new PropertyControlSettings();

        public Object[] InnerValues { get; set; }
        public IPropertyControlSettings SetInnerValues(Object[] newInnerValues)
        {
            PropertyControlSettings pcs = new PropertyControlSettings(this);
            pcs.InnerValues = newInnerValues;
            return pcs;
        }

        public String DisplayMemberPath { get; set; }
        public IPropertyControlSettings SetDisplayMemberPath(String newDisplayMemberPath)
        {
            PropertyControlSettings pcs = new PropertyControlSettings(this);
            pcs.DisplayMemberPath = newDisplayMemberPath;
            return pcs;
        }

        public String PathValue { get; set; }
        public IPropertyControlSettings SetPathValue(String newPathValue)
        {
            PropertyControlSettings pcs = new PropertyControlSettings(this);
            pcs.PathValue = newPathValue;
            return pcs;
        }

        public String Label { get; set; }
        public IPropertyControlSettings SetLabel(String newLabel)
        {
            PropertyControlSettings pcs = new PropertyControlSettings(this);
            pcs.Label = newLabel;
            return pcs;
        }

        public double? MinWidthLabel { get; set; }
        public IPropertyControlSettings SetMinWidthLabel(double? newMinWidthLabel)
        {
            PropertyControlSettings pcs = new PropertyControlSettings(this);
            pcs.MinWidthLabel = newMinWidthLabel;
            return pcs;
        }

        public Boolean? Enabled { get; set; }
        public IPropertyControlSettings SetEnabled(Boolean newEnabled)
        {
            PropertyControlSettings pcs = new PropertyControlSettings(this);
            pcs.Enabled = newEnabled;
            return pcs;
        }

        public Boolean? ReadOnly { get; set; }
        public IPropertyControlSettings SetReadOnly(Boolean newReadOnly)
        {
            PropertyControlSettings pcs = new PropertyControlSettings(this);
            pcs.ReadOnly = newReadOnly;
            return pcs;
        }

        public String ControlToolTipText { get; set; }
        public IPropertyControlSettings SetControlToolTipText(String newControlToolTipText)
        {
            PropertyControlSettings pcs = new PropertyControlSettings(this);
            pcs.ControlToolTipText = newControlToolTipText;
            return pcs;
        }

        public double HeightMultiline { get; set; }
        public IPropertyControlSettings SetHeightMultiline(double newHeightMultiline)
        {
            PropertyControlSettings pcs = new PropertyControlSettings(this);
            pcs.HeightMultiline = newHeightMultiline;
            return pcs;
        }

        public int? SelectedIndex { get; set; }
        public IPropertyControlSettings SetSelectedIndex(int? newSelectedIndex)
        {
            PropertyControlSettings pcs = new PropertyControlSettings(this);
            pcs.SelectedIndex = newSelectedIndex;
            return pcs;
        }

        public int ColumnSpan { get; set; }
        public IPropertyControlSettings SetColumnSpan(int newColumnSpan)
        {
            PropertyControlSettings pcs = new PropertyControlSettings(this);
            pcs.ColumnSpan = newColumnSpan;
            return pcs;
        }

        public Func<object, bool> Validate { get; set; }
        public IPropertyControlSettings SetValidate(Func<Object, Boolean> newValidate)
        {
            PropertyControlSettings pcs = new PropertyControlSettings(this);
            pcs.Validate = newValidate;
            return pcs;
        }

        public Action<PropertyControl> OnValid { get; set; }
        public IPropertyControlSettings SetOnValid(Action<PropertyControl> newOnvalid)
        {
            PropertyControlSettings pcs = new PropertyControlSettings(this);
            pcs.OnValid = newOnvalid;
            return pcs;
        }

        public Action<PropertyControl> OnInvalid { get; set; }
        public IPropertyControlSettings SetOnInvalid(Action<PropertyControl> newOnInvalid)
        {
            PropertyControlSettings pcs = new PropertyControlSettings(this);
            pcs.OnInvalid = newOnInvalid;
            return pcs;
        }

        public Type Type { get; set; }
        public IPropertyControlSettings SetType(Type newType)
        {
            PropertyControlSettings pcs = new PropertyControlSettings(this);
            pcs.Type = newType;
            return pcs;
        }

        public SelectionChangedEventHandler SelectionChanged { get; set; }
        public IPropertyControlSettings AddSelectionChanged(SelectionChangedEventHandler newSelectionChanged)
        {
            PropertyControlSettings pcs = new PropertyControlSettings(this);
            pcs.SelectionChanged = newSelectionChanged;
            return pcs;
        }

        public RoutedPropertyChangedEventHandler<object> ValueChanged { get; set; }
        public IPropertyControlSettings AddValueChanged(RoutedPropertyChangedEventHandler<object> newValueChanged)
        {
            PropertyControlSettings pcs = new PropertyControlSettings(this);
            pcs.ValueChanged = newValueChanged;
            return pcs;
        }

        public RoutedEventHandler CheckedChanged { get; set; }
        public IPropertyControlSettings AddCheckedChanged(RoutedEventHandler newCheckedChanged)
        {
            PropertyControlSettings pcs = new PropertyControlSettings(this);
            pcs.CheckedChanged = newCheckedChanged;
            return pcs;
        }

        public TextChangedEventHandler TextChanged { get; set; }
        public IPropertyControlSettings AddTextChanged(TextChangedEventHandler newTextChanged)
        {
            PropertyControlSettings pcs = new PropertyControlSettings(this);
            pcs.TextChanged = newTextChanged;
            return pcs;
        }

        public KeyEventHandler KeyDown { get; set; }
        public IPropertyControlSettings AddKeyDown(KeyEventHandler newKeyDown)
        {
            PropertyControlSettings pcs = new PropertyControlSettings(this);
            pcs.KeyDown = newKeyDown;
            return pcs;
        }

        public String DesignPath { get; set; }
        public IPropertyControlSettings SetDesignPath(String newDesignPath)
        {
            PropertyControlSettings pcs = new PropertyControlSettings(this);
            pcs.DesignPath = newDesignPath;
            return pcs;
        }

        public Action<object, RoutedEventArgs> Click { get; set; }
        public IPropertyControlSettings AddClick(Action<object, RoutedEventArgs> newClick)
        {
            PropertyControlSettings pcs = new PropertyControlSettings(this);
            pcs.Click = newClick;
            return pcs;
        }

        public Action<object, KeyEventArgs> KeyDownCombo { get; set; }
        public IPropertyControlSettings AddKeyDownCombo(Action<object, KeyEventArgs> newKeyDown)
        {
            PropertyControlSettings pcs = new PropertyControlSettings(this);
            pcs.KeyDownCombo = newKeyDown;
            return pcs;
        }

        public Brush BackgroundColor { get; set; }
        public IPropertyControlSettings SetColor(Brush newBackgroundColor)
        {
            PropertyControlSettings pcs = new PropertyControlSettings(this);
            pcs.BackgroundColor = newBackgroundColor;
            return pcs;
        }

        public HorizontalAlignment? HorizontalAlignment { get; set; }
        public IPropertyControlSettings SetHorizontalAlignment(HorizontalAlignment newHorizontalAlignment)
        {
            PropertyControlSettings pcs = new PropertyControlSettings(this);
            pcs.HorizontalAlignment = newHorizontalAlignment;
            return pcs;
        }

        public Object TargetNull { get; set; }
        public IPropertyControlSettings SetTargetNull(Object newTargetNull)
        {
            PropertyControlSettings pcs = new PropertyControlSettings(this);
            pcs.TargetNull = newTargetNull;
            return pcs;
        }

        public PropertyControlSettings()
        {
        }

        public PropertyControlSettings(PropertyControlSettings copy)
        {
            InnerValues = copy.InnerValues;
            DisplayMemberPath = copy.DisplayMemberPath;
            PathValue = copy.PathValue;
            Label = copy.Label;
            MinWidthLabel = copy.MinWidthLabel;
            Enabled = copy.Enabled;
            ReadOnly = copy.ReadOnly;
            ControlToolTipText = copy.ControlToolTipText;
            HeightMultiline = copy.HeightMultiline;
            SelectedIndex = copy.SelectedIndex;
            ColumnSpan = copy.ColumnSpan;
            Validate = copy.Validate;
            OnValid = copy.OnValid;
            OnInvalid = copy.OnInvalid;
            Type = copy.Type;
            SelectionChanged = copy.SelectionChanged;
            ValueChanged = copy.ValueChanged;
            CheckedChanged = copy.CheckedChanged;
            TextChanged = copy.TextChanged;
            KeyDown = copy.KeyDown;
            DesignPath = copy.DesignPath;
            Click = copy.Click;
            KeyDownCombo = copy.KeyDownCombo;
            BackgroundColor = copy.BackgroundColor;
            HorizontalAlignment = copy.HorizontalAlignment;
            TargetNull = copy.TargetNull;
        }
    }

}
