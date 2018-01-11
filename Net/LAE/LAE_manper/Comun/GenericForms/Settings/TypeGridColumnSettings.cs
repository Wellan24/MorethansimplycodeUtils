using GenericForms.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericForms.Settings
{
    public class TypeGridColumnSettings : ITypeGridColumnSettings
    {
        public string Label { get; set; }
        public ITypeGridColumnSettings SetLabel(string newLabel)
        {
            TypeGridColumnSettings tgcs = new TypeGridColumnSettings(this);
            tgcs.Label = newLabel;
            return tgcs;
        }

        public double? Width { get; set; }
        public ITypeGridColumnSettings SetWidth(double newWidth)
        {
            TypeGridColumnSettings tgcs = new TypeGridColumnSettings(this);
            tgcs.Width = newWidth;
            return tgcs;
        }

        public ColumnLengthUnitType LengthUnitType { get; set; }
        public ITypeGridColumnSettings SetLengthUnitType(ColumnLengthUnitType newLengthUnitType)
        {
            TypeGridColumnSettings tgcs = new TypeGridColumnSettings(this);
            tgcs.LengthUnitType = newLengthUnitType;
            return tgcs;
        }

        public ITypeGCButtonSettings ColumnButton { get; set; }
        public ITypeGridColumnSettings SetColumnButton(ITypeGCButtonSettings newColumnButton)
        {
            TypeGridColumnSettings tgcs = new TypeGridColumnSettings(this);
            tgcs.ColumnButton = newColumnButton;
            return tgcs;
        }

        public ITypeGCComboSettings ColumnCombo { get; set; }
        public ITypeGridColumnSettings SetColumnCombo(ITypeGCComboSettings newColumnCombo)
        {
            TypeGridColumnSettings tgcs = new TypeGridColumnSettings(this);
            tgcs.ColumnCombo = newColumnCombo;
            return tgcs;
        }

        public String Format { get; set; }
        public ITypeGridColumnSettings SetFormat(String newFormat)
        {
            TypeGridColumnSettings tgcs = new TypeGridColumnSettings(this);
            tgcs.Format = newFormat;
            return tgcs;
        }

        public TypeGridColumnSettings()
        {

        }

        public TypeGridColumnSettings(TypeGridColumnSettings copy)
        {
            Label = copy.Label;
            Width = copy.Width;
            LengthUnitType = copy.LengthUnitType;
            ColumnButton = copy.ColumnButton;
            ColumnCombo = copy.ColumnCombo;
            Format = copy.Format;
        }
    }
}
