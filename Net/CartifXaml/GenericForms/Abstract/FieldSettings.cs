using Cartif.Collections;
using GenericForms.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericForms.Abstract
{
    public class FieldSettings : CartifDictionary<String, IPropertyControlSettings>
    {
        public FieldSettings() : base(4) { }
        public FieldSettings(int initialCapacity) : base(initialCapacity) { }
    }
}
