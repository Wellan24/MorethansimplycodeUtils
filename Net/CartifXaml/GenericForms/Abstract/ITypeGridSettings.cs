using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericForms.Abstract
{
    public interface ITypeGridSettings
    {
        ColumnGridSettings Columns { get; set; }
        ITypeGridSettings SetColumns(ColumnGridSettings newColumns);

        //String Label { get; set; }
        //ITypeGridSettings SetLabel(String newLabel);

        //double? Width { get; set; }
        //ITypeGridSettings SetWidth(double newWidth);

    }
}
