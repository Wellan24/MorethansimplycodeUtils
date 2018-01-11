using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericForms.Settings
{
    class ValidateEnum
    {
        public static Func<object, bool> noEmpty { get; } = ((p) => !String.IsNullOrWhiteSpace(p.ToString()));

    }
}
