using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GenericForms.Settings
{
    class ValidateEnum
    {
        public static Func<object, bool> noEmpty { get; } = ((p) => !String.IsNullOrWhiteSpace(p?.ToString()));
        public static Func<object, bool> isValidEmail { get; } = (p)=>{
            //try {
            //    var addr = new System.Net.Mail.MailAddress(p.ToString());
            //    return addr.Address == p.ToString();
            //}
            //catch
            //{
            //    return false;
            //}
            Regex rgx = new Regex(@"^.*[^\.]@[^\.]+(?:\.[^.]+)+$");
            return rgx.IsMatch(p.ToString());
        };
        public static Func<object, bool> isValidEmailOrEmpty { get; } = (p) => {
            if (p == null || p.Equals(""))
                return true;
            return isValidEmail(p);
        };
    }
}
