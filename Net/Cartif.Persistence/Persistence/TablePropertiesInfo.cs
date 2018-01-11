using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class TablePropertiesInfo
    {
        public readonly String DbName;
        public readonly String ClassName;

        public TablePropertiesInfo(String DbName, String PropertyName)
        {
            this.DbName = DbName;
            this.ClassName = PropertyName;
        }
    }
}
