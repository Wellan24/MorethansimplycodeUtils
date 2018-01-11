using GenericForms.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericForms.Settings
{
    class TypeGridColumnSettings : ITypeGridColumnSettings
    {
        public string Label { get; set; }
        public ITypeGridColumnSettings SetLabel(string newLabel)
        {
            // TypeGridColumnSettings this = new TypeGridColumnSettings(this);
            this.Label = newLabel;
            return this;
        }

        public double? Width { get; set; }
        public ITypeGridColumnSettings SetWidth(double newWidth)
        {
            // TypeGridColumnSettings this = new TypeGridColumnSettings(this);
            this.Width = newWidth;
            return this;
        }

        public ColumnLengthUnitType LengthUnitType { get; set; }
        public ITypeGridColumnSettings SetLengthUnitType(ColumnLengthUnitType newLengthUnitType)
        {
            // TypeGridColumnSettings this = new TypeGridColumnSettings(this);
            this.LengthUnitType = newLengthUnitType;
            return this;
        }

        public ITypeGCButtonSettings ColumnButton { get; set; }
        public ITypeGridColumnSettings SetColumnButton(ITypeGCButtonSettings newColumnButton)
        {
            // TypeGridColumnSettings this = new TypeGridColumnSettings(this);
            this.ColumnButton = newColumnButton;
            return this;
        }

        public ITypeGCComboSettings ColumnCombo { get; set; }
        public ITypeGridColumnSettings SetColumnCombo(ITypeGCComboSettings newColumnCombo)
        {
            // TypeGridColumnSettings this = new TypeGridColumnSettings(this);
            this.ColumnCombo = newColumnCombo;
            return this;
        }

        public String Format { get; set; }
        public ITypeGridColumnSettings SetFormat(String newFormat)
        {
            // TypeGridColumnSettings this = new TypeGridColumnSettings(this);
            this.Format = newFormat;
            return this;
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
