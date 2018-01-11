using GenericForms.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
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

        internal object SetInnerValues(object recuperarClientes)
        {
            throw new NotImplementedException();
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

        public PropertyControlSettings()
        {
        }

        public PropertyControlSettings(PropertyControlSettings copy)
        {
            InnerValues = copy.InnerValues;
            PathValue = copy.PathValue;
            Label = copy.Label;
            Enabled = copy.Enabled;
            ControlToolTipText = copy.ControlToolTipText;
            HeightMultiline = copy.HeightMultiline;
            ColumnSpan = copy.ColumnSpan;
            Validate = copy.Validate;
            OnValid = copy.OnValid;
            OnInvalid = copy.OnInvalid;
            Type = copy.Type;
            SelectionChanged = copy.SelectionChanged;
        }
    }

}
