using Cartif.Expectation;
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
            // TypePanelSettings<T> this = new TypePanelSettings<T>(this);
            this.DefaultSettings = newSettings;
            return this;
        }

        public FieldSettings Fields { get; set; }
        public ITypePanelSettings<T> SetFields(FieldSettings newFields)
        {
            // TypePanelSettings<T> this = new TypePanelSettings<T>(this);
            this.Fields = newFields;
            return this;
        }

        public AbstractExpectation<T> PanelValidation { get; set; }
        public ITypePanelSettings<T> SetValidationPanel(AbstractExpectation<T> newValidation)
        {
            // TypePanelSettings<T> this = new TypePanelSettings<T>(this);
            this.PanelValidation = newValidation;
            return this;
        }

        public int[] ColumnWidths { get; set; }
        public ITypePanelSettings<T> SetColumnWidths(int[] newColumnWidths)
        {
            // TypePanelSettings<T> this = new TypePanelSettings<T>(this);
            this.ColumnWidths = newColumnWidths;
            return this;
        }

        public Boolean IsUpdating { get; set; }
        public ITypePanelSettings<T> SetIsUpdating(Boolean newValue)
        {
            // TypePanelSettings<T> this = new TypePanelSettings<T>(this);
            this.IsUpdating = newValue;
            return this;
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
