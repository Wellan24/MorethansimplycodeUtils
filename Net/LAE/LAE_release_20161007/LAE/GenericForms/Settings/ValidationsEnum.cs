using GenericForms.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GenericForms.Settings
{
    class ValidationsEnum
    {
        public static Action<PropertyControl> DefaultWrong { get; } = ((p) => p.ShowMessage("Incorrecto", Brushes.Red));
        public static Action<PropertyControl> RightWithoutMessage { get; } = ((p) => p.ShowMessage("", Brushes.Black));
        public static Action<PropertyControl> DefaultRight { get; } = ((p) => p.ShowMessage("Correcto", Brushes.Green));
    }
}
