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

        public ColumnGridSettings Columns { get; set; }
        public ITypeGridSettings SetColumns(ColumnGridSettings newColumns)
        {
            TypeGridSettings tgs = new TypeGridSettings(this);
            tgs.Columns = newColumns;
            return tgs;
        }

        //public String Label { get; set; }

        //public ITypeGridSettings SetLabel(string newLabel)
        //{
        //    TypeGridSettings dgs = new TypeGridSettings(this);
        //    dgs.Label = newLabel;
        //    return dgs;
        //}

        //public double? Width { get; set; }
        //public ITypeGridSettings SetWidth(double newWidth)
        //{
        //    TypeGridSettings dgs = new TypeGridSettings(this);
        //    dgs.Width = newWidth;
        //    return dgs;
        //}

        public TypeGridSettings()
        {

        }

        public TypeGridSettings(TypeGridSettings copy)
        {
            Columns = copy.Columns;
            //Label = copy.Label;
            //Width = copy.Width;
        }
    }
}
