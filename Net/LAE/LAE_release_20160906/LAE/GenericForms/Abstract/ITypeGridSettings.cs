using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericForms.Abstract
{
    public interface ITypeGridSettings
    {
        ITypeGridColumnSettings DefaultSettings { get; set; }
        ITypeGridSettings SetDefaultSettings(ITypeGridColumnSettings newSettings);

        ColumnGridSettings Columns { get; set; }
        ITypeGridSettings SetColumns(ColumnGridSettings newColumns);

        Boolean? CanResizeColumns { get; set; }
        ITypeGridSettings SetCanResizeColumns(Boolean newValue);

        Boolean? CanReorderColumns { get; set; }
        ITypeGridSettings SetCanReorderColumns(Boolean newValue);

        Boolean? Editable { get; set; }
        ITypeGridSettings SetEditable(Boolean newValue);

        Func<Object, bool> LoadGrid { get; set; }
        ITypeGridSettings SetLoadGrid(Func<Object, bool> newLoadGrid);
    }
}
