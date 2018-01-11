using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Comun.Persistence
{
    public class ColumnPropertiesInfo
    {
        public readonly String DbName;
        public readonly String Format;
        public readonly String PropertyName;
        public readonly Boolean IsId;
        public readonly Boolean IsAutonumeric;

        public ColumnPropertiesInfo(String propertyName, ColumnPropertiesAttribute att)
            : this(att.Name, att.Format, propertyName, att.IsId, att.IsAutonumeric)
        { }

        public ColumnPropertiesInfo(String DbName, String Format, String PropertyName, Boolean IsId = false, Boolean IsAutonumeric = false)
        {
            this.DbName = DbName;
            this.Format = Format;
            this.PropertyName = PropertyName;
            this.IsId = IsId;
            this.IsAutonumeric = IsAutonumeric;
        }
    }
}
