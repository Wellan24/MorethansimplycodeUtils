using Cartif.Util;
using GenericForms.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericForms.Settings
{
    public class TypePanelSettings<T> : ITypePanelSettings<T>
    {
        public IPropertyControlSettings DefaultSettings { get; set; }
        public ITypePanelSettings<T> SetDefaultSettings(IPropertyControlSettings newSettings)
        {
            TypePanelSettings<T> pcs = new TypePanelSettings<T>(this);
            pcs.DefaultSettings = newSettings;
            return pcs;
        }

        public FieldSettings Fields { get; set; }
        public ITypePanelSettings<T> SetFields(FieldSettings newFields)
        {
            TypePanelSettings<T> pcs = new TypePanelSettings<T>(this);
            pcs.Fields = newFields;
            return pcs;
        }

        public Expectation<T> PanelValidation { get; set; }
        public ITypePanelSettings<T> SetValidationPanel(Expectation<T> newValidation)
        {
            TypePanelSettings<T> pcs = new TypePanelSettings<T>(this);
            pcs.PanelValidation = newValidation;
            return pcs;
        }


        public TypePanelSettings()
        {
        }

        public TypePanelSettings(ITypePanelSettings<T> copy)
        {
            DefaultSettings = copy.DefaultSettings;
            Fields = copy.Fields;
            PanelValidation = copy.PanelValidation;
        }
    }
}
