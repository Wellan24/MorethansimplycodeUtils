using GenericForms.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericForms.Settings
{
    class TypeGridColumnSettingsEnum
    {
        public static TypeGridColumnSettings DefaultColum
        { get { return new TypeGridColumnSettings(defaultColumn); } }

        private static readonly TypeGridColumnSettings defaultColumn = new TypeGridColumnSettings
        {
            Width = 1,
            LengthUnitType= ColumnLengthUnitType.Star
        };
    }
}
