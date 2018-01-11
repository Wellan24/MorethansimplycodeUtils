using System;
using System.ComponentModel;

namespace Cartif.IO
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A path converter. </summary>
    /// <remarks> Oscvic, 2016-01-04. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class PathConverter : TypeConverter
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Determine if we can convert from. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="context">    Objeto <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> que
        ///                           proporciona un contexto de formato. </param>
        /// <param name="sourceType"> <see cref="T:System.Type" /> que representa el tipo a partir del cual se
        ///                           desea realizar la conversión. </param>
        /// <returns> true si este convertidor puede realizar la conversión; en caso contrario, false. </returns>
        ///--------------------------------------------------------------------------------------------------
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Convert from. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="context"> Objeto <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> que
        ///                        proporciona un contexto de formato. </param>
        /// <param name="culture"> <see cref="T:System.Globalization.CultureInfo" /> que se va a utilizar
        ///                        como la referencia cultural actual. </param>
        /// <param name="value">   <see cref="T:System.Object" /> que se va a convertir. </param>
        /// <returns> <see cref="T:System.Object" /> que representa el valor convertido. </returns>
        ///--------------------------------------------------------------------------------------------------
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            var valueString = value as string;
            return valueString != null ? new Path(valueString) : base.ConvertFrom(context, culture, value);
        }
    }
}
