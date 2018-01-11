using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GenericForms.Abstract
{
    public class GenericValidation<PropertyControl> : ValidationRule
    {
        Func<object, Boolean> validate;
        Action<PropertyControl> onValid;
        Action<PropertyControl> onInvalid;
        PropertyControl pc;

        public GenericValidation(PropertyControl pc, Func<object, Boolean> validate, Action<PropertyControl> onValid, Action<PropertyControl> onInvalid)
        {
            this.pc = pc;
            this.validate = validate;
            this.onValid = onValid;
            this.onInvalid = onInvalid;
        }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (validate != null)
            {
                Boolean valido = validate(value);

                if (valido)
                {
                    onValid?.Invoke(pc);
                    return new ValidationResult(true, value);
                }
                else {
                    onInvalid?.Invoke(pc);
                    return new ValidationResult(false, value);
                }
            }
            return new ValidationResult(true, value);
        }

    }
}
