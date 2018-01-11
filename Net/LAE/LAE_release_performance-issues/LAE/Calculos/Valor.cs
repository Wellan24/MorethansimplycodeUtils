using LAE.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cartif.Extensions;

namespace LAE.Calculos
{
    public partial class Valor
    {
        public double Value { get; private set; }
        public Unidad Unidad { get; private set; }

        private Valor(double value, Unidad unidad)
        {
            Value = value;
            Unidad = unidad;
        }

        public static Valor Of(double? value, Unidad unidad)
        {
            return innerOf(value, unidad);
        }

        public static Valor Of(double? value, String unidad)
        {
            return innerOf(value, Unidad.Of(unidad));
        }

        public static Valor Of(double? value, int idUnidad)
        {
            return innerOf(value, Unidad.Of(idUnidad));
        }

        public static Valor innerOf(double? value, Unidad unidad)
        {
            if (value == null)
                return null;
            return new Valor(value ?? 0, unidad);
        }

        public override string ToString()
        {
            return Value + " " + Unidad.Abreviatura;
        }
    }
}
