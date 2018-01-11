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
            TypePanelSettings<T> tps = new TypePanelSettings<T>(this);
            tps.DefaultSettings = newSettings;
            return tps;
        }

        public FieldSettings Fields { get; set; }
        public ITypePanelSettings<T> SetFields(FieldSettings newFields)
        {
            TypePanelSettings<T> tps = new TypePanelSettings<T>(this);
            tps.Fields = newFields;
            return tps;
        }

        public Expectation<T> PanelValidation { get; set; }
        public ITypePanelSettings<T> SetValidationPanel(Expectation<T> newValidation)
        {
            TypePanelSettings<T> tps = new TypePanelSettings<T>(this);
            tps.PanelValidation = newValidation;
            return tps;
        }

        public int[] ColumnWidths { get; set; }
        public ITypePanelSettings<T> SetColumnWidths(int[] newColumnWidths)
        {
            TypePanelSettings<T> tps = new TypePanelSettings<T>(this);
            tps.ColumnWidths = newColumnWidths;
            return tps;
        }

        public Boolean IsUpdating { get; set; }
        public ITypePanelSettings<T> SetIsUpdating(Boolean newValue)
        {
            TypePanelSettings<T> tps = new TypePanelSettings<T>(this);
            tps.IsUpdating = newValue;
            return tps;
        }

        public TypePanelSettings()
        {
        }

        public TypePanelSettings(ITypePanelSettings<T> copy)
        {
            DefaultSettings = copy.DefaultSettings;
            Fields = copy.Fields;
            PanelValidation = copy.PanelValidation;
            ColumnWidths = copy.ColumnWidths;
            IsUpdating = copy.IsUpdating;
        }
    }
}
