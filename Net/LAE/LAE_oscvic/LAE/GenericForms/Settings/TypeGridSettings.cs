using GenericForms;
using GenericForms.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericForms.Settings
{
    class TypeGridSettings : ITypeGridSettings
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

        public Func<Object, bool> LoadGrid { get; set; }
        public ITypeGridSettings SetLoadGrid(Func<Object,bool> newLoadGrid)
        {
            TypeGridSettings tgs = new TypeGridSettings(this);
            tgs.LoadGrid = newLoadGrid;
            return tgs;
        }

        public TypeGridSettings() { }

        public TypeGridSettings(TypeGridSettings copy)
        {
            DefaultSettings = copy.DefaultSettings;
            Columns = copy.Columns;
            CanResizeColumns = copy.CanResizeColumns;
            CanReorderColumns = copy.CanReorderColumns;
            LoadGrid = copy.LoadGrid;
        }
    }
}
