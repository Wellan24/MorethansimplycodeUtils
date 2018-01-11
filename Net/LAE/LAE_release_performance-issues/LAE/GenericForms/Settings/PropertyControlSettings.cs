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
            // PropertyControlSettings this = new PropertyControlSettings(this);
            this.InnerValues = newInnerValues;
            return this;
        }

        public String DisplayMemberPath { get; set; }
        public IPropertyControlSettings SetDisplayMemberPath(String newDisplayMemberPath)
        {
            // PropertyControlSettings this = new PropertyControlSettings(this);
            this.DisplayMemberPath = newDisplayMemberPath;
            return this;
        }

        public String PathValue { get; set; }
        public IPropertyControlSettings SetPathValue(String newPathValue)
        {
            // PropertyControlSettings this = new PropertyControlSettings(this);
            this.PathValue = newPathValue;
            return this;
        }

        public String Label { get; set; }
        public IPropertyControlSettings SetLabel(String newLabel)
        {
            // PropertyControlSettings this = new PropertyControlSettings(this);
            this.Label = newLabel;
            return this;
        }

        public Boolean? Enabled { get; set; }
        public IPropertyControlSettings SetEnabled(Boolean newEnabled)
        {
            // PropertyControlSettings this = new PropertyControlSettings(this);
            this.Enabled = newEnabled;
            return this;
        }

        public Boolean? ReadOnly { get; set; }
        public IPropertyControlSettings SetReadOnly(Boolean newReadOnly)
        {
            // PropertyControlSettings this = new PropertyControlSettings(this);
            this.ReadOnly = newReadOnly;
            return this;
        }

        public String ControlToolTipText { get; set; }
        public IPropertyControlSettings SetControlToolTipText(String newControlToolTipText)
        {
            // PropertyControlSettings this = new PropertyControlSettings(this);
            this.ControlToolTipText = newControlToolTipText;
            return this;
        }

        public double HeightMultiline { get; set; }
        public IPropertyControlSettings SetHeightMultiline(double newHeightMultiline)
        {
            // PropertyControlSettings this = new PropertyControlSettings(this);
            this.HeightMultiline = newHeightMultiline;
            return this;
        }

        public int? SelectedIndex { get; set; }
        public IPropertyControlSettings SetSelectedIndex(int? newSelectedIndex)
        {
            // PropertyControlSettings this = new PropertyControlSettings(this);
            this.SelectedIndex = newSelectedIndex;
            return this;
        }

        public int ColumnSpan { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Sets column span. </summary>
        /// <remarks> Oscvic, 30/09/2016. </remarks>
        /// <param name="newColumnSpan"> The new column span. </param>
        /// <returns> The IPropertyControlSettings. </returns>
        ///-------------------------------------------------------------------------------------------------
        public IPropertyControlSettings SetColumnSpan(int newColumnSpan)
        {
            // PropertyControlSettings this = new PropertyControlSettings(this);
            this.ColumnSpan = newColumnSpan;
            return this;
        }

        public Func<object, bool> Validate { get; set; }
        public IPropertyControlSettings SetValidate(Func<Object, Boolean> newValidate)
        {
            // PropertyControlSettings this = new PropertyControlSettings(this);
            this.Validate = newValidate;
            return this;
        }

        public Action<PropertyControl> OnValid { get; set; }
        public IPropertyControlSettings SetOnValid(Action<PropertyControl> newOnvalid)
        {
            // PropertyControlSettings this = new PropertyControlSettings(this);
            this.OnValid = newOnvalid;
            return this;
        }

        public Action<PropertyControl> OnInvalid { get; set; }
        public IPropertyControlSettings SetOnInvalid(Action<PropertyControl> newOnInvalid)
        {
            // PropertyControlSettings this = new PropertyControlSettings(this);
            this.OnInvalid = newOnInvalid;
            return this;
        }

        public Type Type { get; set; }
        public IPropertyControlSettings SetType(Type newType)
        {
            // PropertyControlSettings this = new PropertyControlSettings(this);
            this.Type = newType;
            return this;
        }

        public SelectionChangedEventHandler SelectionChanged { get; set; }
        public IPropertyControlSettings AddSelectionChanged(SelectionChangedEventHandler newSelectionChanged)
        {
            // PropertyControlSettings this = new PropertyControlSettings(this);
            this.SelectionChanged = newSelectionChanged;
            return this;
        }

        public RoutedPropertyChangedEventHandler<object> ValueChanged { get; set; }
        public IPropertyControlSettings AddValueChanged(RoutedPropertyChangedEventHandler<object> newValueChanged)
        {
            // PropertyControlSettings this = new PropertyControlSettings(this);
            this.ValueChanged = newValueChanged;
            return this;
        }

        public RoutedEventHandler CheckedChanged { get; set; }
        public IPropertyControlSettings AddCheckedChanged(RoutedEventHandler newCheckedChanged)
        {
            // PropertyControlSettings this = new PropertyControlSettings(this);
            this.CheckedChanged = newCheckedChanged;
            return this;
        }

        public TextChangedEventHandler TextChanged { get; set; }
        public IPropertyControlSettings AddTextChanged(TextChangedEventHandler newTextChanged)
        {
            // PropertyControlSettings this = new PropertyControlSettings(this);
            this.TextChanged = newTextChanged;
            return this;
        }

        public KeyEventHandler KeyDown { get; set; }
        public IPropertyControlSettings AddKeyDown(KeyEventHandler newKeyDown)
        {
            // PropertyControlSettings this = new PropertyControlSettings(this);
            this.KeyDown = newKeyDown;
            return this;
        }

        public String DesignPath { get; set; }
        public IPropertyControlSettings SetDesignPath(String newDesignPath)
        {
            // PropertyControlSettings this = new PropertyControlSettings(this);
            this.DesignPath = newDesignPath;
            return this;
        }

        public Action<object, RoutedEventArgs> Click { get; set; }
        public IPropertyControlSettings AddClick(Action<object, RoutedEventArgs> newClick)
        {
            // PropertyControlSettings this = new PropertyControlSettings(this);
            this.Click = newClick;
            return this;
        }

        public Action<object, KeyEventArgs> KeyDownCombo { get; set; }
        public IPropertyControlSettings AddKeyDownCombo(Action<object, KeyEventArgs> newKeyDown)
        {
            // PropertyControlSettings this = new PropertyControlSettings(this);
            this.KeyDownCombo = newKeyDown;
            return this;
        }

        public Brush BackgroundColor { get; set; }
        public IPropertyControlSettings SetColor(Brush newBackgroundColor)
        {
            // PropertyControlSettings this = new PropertyControlSettings(this);
            this.BackgroundColor = newBackgroundColor;
            return this;
        }

        public HorizontalAlignment? HorizontalAlignment { get; set; }
        public IPropertyControlSettings SetHorizontalAlignment(HorizontalAlignment newHorizontalAlignment)
        {
            // PropertyControlSettings this = new PropertyControlSettings(this);
            this.HorizontalAlignment = newHorizontalAlignment;
            return this;
        }

        public Object TargetNull { get; set; }
        public IPropertyControlSettings SetTargetNull(Object newTargetNull)
        {
            // PropertyControlSettings this = new PropertyControlSettings(this);
            this.TargetNull = newTargetNull;
            return this;
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
