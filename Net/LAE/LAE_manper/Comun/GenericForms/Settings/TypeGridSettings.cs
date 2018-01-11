using GenericForms;
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
    public class TypeGridSettings : ITypeGridSettings
    {
        public ITypeGridColumnSettings DefaultSettings { get; set; }
        public ITypeGridSettings SetDefaultSettings(ITypeGridColumnSettings newSettings)
        {
            TypeGridSettings tgs = new TypeGridSettings(this);
            tgs.DefaultSettings = newSettings;
            return tgs;
        }

        public ColumnGridSettings Columns { get; set; }
        public ITypeGridSettings SetColumns(ColumnGridSettings newColumns)
        {
            TypeGridSettings tgs = new TypeGridSettings(this);
            tgs.Columns = newColumns;
            return tgs;
        }


        public Boolean? CanResizeColumns { get; set; }
        public ITypeGridSettings SetCanResizeColumns(Boolean newValue)
        {
            TypeGridSettings tgs = new TypeGridSettings(this);
            tgs.CanResizeColumns = newValue;
            return tgs;
        }

        public Boolean? CanReorderColumns { get; set; }
        public ITypeGridSettings SetCanReorderColumns(Boolean newValue)
        {
            TypeGridSettings tgs = new TypeGridSettings(this);
            tgs.CanReorderColumns = newValue;
            return tgs;
        }

        public Boolean? CanSortColumns { get; set; }
        public ITypeGridSettings SetCanSortColumns(Boolean newValue)
        {
            TypeGridSettings tgs = new TypeGridSettings(this);
            tgs.CanSortColumns = newValue;
            return tgs;
        }

        public Boolean? Editable { get; set; }
        public ITypeGridSettings SetEditable(Boolean newValue)
        {
            TypeGridSettings tgs = new TypeGridSettings(this);
            tgs.Editable = newValue;
            return tgs;
        }

        public Func<Object, SolidColorBrush> ForegroundRow { get; set; }
        public ITypeGridSettings SetForegroundRow(Func<Object, SolidColorBrush> newForegroundRow)
        {
            TypeGridSettings tgs = new TypeGridSettings(this);
            tgs.ForegroundRow = newForegroundRow;
            return tgs;
        }

        public Func<Object, SolidColorBrush> BackgroundRow { get; set; }
        public ITypeGridSettings SetBackgroundRow(Func<Object, SolidColorBrush> newBackgroundRow)
        {
            TypeGridSettings tgs = new TypeGridSettings(this);
            tgs.BackgroundRow = newBackgroundRow;
            return tgs;
        }

        public SelectionChangedEventHandler SelectionChanged { get; set; }
        public ITypeGridSettings AddSelectionChanged(SelectionChangedEventHandler newSelectionChanged)
        {
            TypeGridSettings tgs = new TypeGridSettings(this);
            tgs.SelectionChanged = newSelectionChanged;
            return tgs;
        }


        public TypeGridSettings() { }

        public TypeGridSettings(TypeGridSettings copy)
        {
            DefaultSettings = copy.DefaultSettings;
            Columns = copy.Columns;
            CanResizeColumns = copy.CanResizeColumns;
            CanReorderColumns = copy.CanReorderColumns;
            CanSortColumns = copy.CanSortColumns;
            Editable = copy.Editable;
            ForegroundRow = copy.ForegroundRow;
            BackgroundRow = copy.BackgroundRow;
            SelectionChanged = copy.SelectionChanged;
        }
    }
}
