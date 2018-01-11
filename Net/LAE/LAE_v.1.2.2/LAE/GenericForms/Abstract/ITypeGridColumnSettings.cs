using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GenericForms.Abstract
{
    public interface ITypeGridColumnSettings
    {
        double? Width { get; set; }
        ITypeGridColumnSettings SetWidth(double newWidth);

        String Label { get; set; }
        ITypeGridColumnSettings SetLabel(string newLabel);

        ColumnLengthUnitType LengthUnitType { get; set; }
        ITypeGridColumnSettings SetLengthUnitType(ColumnLengthUnitType newUnit);

        ITypeGCButtonSettings ColumnButton { get; set; }
        ITypeGridColumnSettings SetColumnButton(ITypeGCButtonSettings newColumnButton);

        ITypeGCComboSettings ColumnCombo { get; set; }
        ITypeGridColumnSettings SetColumnCombo(ITypeGCComboSettings newColumnCombo);

        String Format { get; set; }
        ITypeGridColumnSettings SetFormat(String newFormat);
    }

    public class ColumnLengthUnitType
    {
        public static readonly ColumnLengthUnitType Auto = new ColumnLengthUnitType(DataGridLengthUnitType.Auto);
        public static readonly ColumnLengthUnitType Pixel = new ColumnLengthUnitType(DataGridLengthUnitType.Pixel);
        public static readonly ColumnLengthUnitType SizeToCells = new ColumnLengthUnitType(DataGridLengthUnitType.SizeToCells);
        public static readonly ColumnLengthUnitType SizeToHeader = new ColumnLengthUnitType(DataGridLengthUnitType.SizeToHeader);
        public static readonly ColumnLengthUnitType Star = new ColumnLengthUnitType(DataGridLengthUnitType.Star);

        public DataGridLengthUnitType LengthUnitType { get; private set; }
        private ColumnLengthUnitType(DataGridLengthUnitType unit)
        {
            LengthUnitType = unit;
        }
    }
}
