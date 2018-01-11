using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.DocWord
{
    class FilaTabla
    {
        private TableRow row;
        private TableRowProperties trProperties;

        public FilaTabla()
        {
            row = new TableRow();
            trProperties = new TableRowProperties();
        }

        public FilaTabla Height(int value)
        {
            trProperties.Append(new TableRowHeight() { Val = Convert.ToUInt32(value), HeightType = HeightRuleValues.AtLeast });
            return this;
        }

        public TableRow Build()
        {
            row.Append(trProperties);
            return row;
        }

        public static FilaTabla Fila()
        {
            return new FilaTabla();
        }
    }
}
