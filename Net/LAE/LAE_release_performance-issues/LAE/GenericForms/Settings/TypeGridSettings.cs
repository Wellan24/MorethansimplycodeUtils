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
            // TypeGridSettings this = new TypeGridSettings(this);
            this.DefaultSettings = newSettings;
            return this;
        }

        public ColumnGridSettings Columns { get; set; }
        public ITypeGridSettings SetColumns(ColumnGridSettings newColumns)
        {
            // TypeGridSettings this = new TypeGridSettings(this);
            this.Columns = newColumns;
            return this;
        }


        public Boolean? CanResizeColumns { get; set; }
        public ITypeGridSettings SetCanResizeColumns(Boolean newValue)
        {
            // TypeGridSettings this = new TypeGridSettings(this);
            this.CanResizeColumns = newValue;
            return this;
        }

        public Boolean? CanReorderColumns { get; set; }
        public ITypeGridSettings SetCanReorderColumns(Boolean newValue)
        {
            // TypeGridSettings this = new TypeGridSettings(this);
            this.CanReorderColumns = newValue;
            return this;
        }

        public Boolean? Editable { get; set; }
        public ITypeGridSettings SetEditable(Boolean newValue)
        {
            // TypeGridSettings this = new TypeGridSettings(this);
            this.Editable = newValue;
            return this;
        }

        public Func<Object, bool> LoadGrid { get; set; }
        public ITypeGridSettings SetLoadGrid(Func<Object,bool> newLoadGrid)
        {
            // TypeGridSettings this = new TypeGridSettings(this);
            this.LoadGrid = newLoadGrid;
            return this;
        }

        public TypeGridSettings() { }

        public TypeGridSettings(TypeGridSettings copy)
        {
            DefaultSettings = copy.DefaultSettings;
            Columns = copy.Columns;
            CanResizeColumns = copy.CanResizeColumns;
            CanReorderColumns = copy.CanReorderColumns;
            Editable = copy.Editable;
            LoadGrid = copy.LoadGrid;
        }
    }
}
