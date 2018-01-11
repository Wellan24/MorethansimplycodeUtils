using Cartif.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericForms.Abstract
{
    public interface ITypePanelSettings<T>
    {
        IPropertyControlSettings DefaultSettings { get; set; }
        ITypePanelSettings<T> SetDefaultSettings(IPropertyControlSettings newSettings);

        FieldSettings Fields { get; set; }
        ITypePanelSettings<T> SetFields(FieldSettings newFields);

        Expectation<T> PanelValidation { get; set; }
        ITypePanelSettings<T> SetValidationPanel(Expectation<T> newValitation);

        int[] ColumnWidths { get; set; }
        ITypePanelSettings<T> SetColumnWidths(int[] newColumnWidths);

        Boolean IsUpdating { get; set; }
        ITypePanelSettings<T> SetIsUpdating(Boolean newValue);
    }
}
