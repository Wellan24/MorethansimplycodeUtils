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

        public String ControlToolTipText { get; set; }
        public IPropertyControlSettings SetControlToolTipText(String newControlToolTipText)
        {
            PropertyControlSettings pcs = new PropertyControlSettings(this);
            pcs.ControlToolTipText = newControlToolTipText;
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

        public PropertyControlSettings()
        {

        }

        public PropertyControlSettings(PropertyControlSettings copy)
        {
            ControlToolTipText = copy.ControlToolTipText;
            InnerValues = copy.InnerValues;
            PathValue = copy.PathValue;
            Label = copy.Label;
            OnValid = copy.OnValid;
            OnInvalid = copy.OnInvalid;
            Type = copy.Type;
            Validate = copy.Validate;
        }
    }

}
