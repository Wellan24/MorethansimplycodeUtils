using LAE.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cartif.Extensions;

namespace LAE.Calculos
{
    static class Conversor
    {
        public static Valor Convert(this Valor value, String unidad) =>
            Convert(value, Unidad.Of(unidad));

        public static Valor Convert(this Valor value, int idUnidad) =>
            Convert(value, Unidad.Of(idUnidad));

        public static Valor Convert(this Valor value) =>
            Convert(value, value.Unidad.BaseOf());

        public static Valor Convert(this Valor value, Unidad unidad) =>
            Valor.Of(value.Value * value.Unidad.FactorConversion / unidad.FactorConversion, unidad);
    }

    public partial class Valor
    {
        void InmutableConvert(Unidad unidad)
        {
            this.Value = this.Value * this.Unidad.FactorConversion / unidad.FactorConversion;
            this.Unidad = unidad;
        }

        void InmutableConvert(String unidad) => InmutableConvert(Unidad.Of(unidad));

        void InmutableConvert(int idUnidad) => InmutableConvert(Unidad.Of(idUnidad));

        void InmutableConvert() => InmutableConvert(this.Unidad.BaseOf());

        public class Conversor
        {
            public static void BatchConvert(params Valor[] valores) =>
                valores.ForEach(rv => rv.InmutableConvert());

            public static void BatchConvert(Unidad unidad, params Valor[] valores) =>
                valores.ForEach(rv => rv.InmutableConvert(unidad));

            public static void BatchConvert(int idUnidad, params Valor[] valores) =>
                valores.ForEach(rv => rv.InmutableConvert(idUnidad));

            public static void BatchConvert(String unidad, params Valor[] valores) =>
                valores.ForEach(rv => rv.InmutableConvert(unidad));
        }
    }
}
