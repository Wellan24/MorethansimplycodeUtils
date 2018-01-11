using Cartif.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericForms.Abstract
{
    public class ColumnGridSettings : CartifDictionary<String, ITypeGridSettings>
    {
        public ColumnGridSettings() : base(4) { }
        public ColumnGridSettings(int initialCapacity) : base(initialCapacity) { }
    }
}
