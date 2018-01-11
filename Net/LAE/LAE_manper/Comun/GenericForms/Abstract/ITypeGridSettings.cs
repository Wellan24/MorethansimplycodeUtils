using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

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

        Boolean? CanSortColumns { get; set; }
        ITypeGridSettings SetCanSortColumns(Boolean newValue);

        Boolean? Editable { get; set; }
        ITypeGridSettings SetEditable(Boolean newValue);

        Func<Object, SolidColorBrush> ForegroundRow { get; set; }
        ITypeGridSettings SetForegroundRow(Func<Object, SolidColorBrush> newForegroundRow);

        Func<Object, SolidColorBrush> BackgroundRow { get; set; }
        ITypeGridSettings SetBackgroundRow(Func<Object, SolidColorBrush> newBackgroundRow);

        SelectionChangedEventHandler SelectionChanged { get; }
        ITypeGridSettings AddSelectionChanged(SelectionChangedEventHandler newSelectionChanged);

    }
}
