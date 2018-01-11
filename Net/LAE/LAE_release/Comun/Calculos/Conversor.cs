using LAE.Comun.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cartif.Extensions;

namespace LAE.Comun.Calculos
{
    public partial class Valor
    {
        void InmutableConvert(Unidad unidad)
        {
            if (!unidad.Equals(this.Unidad))
            {
                this.Value = this.Value * this.Unidad.FactorConversion / unidad.FactorConversion;
                this.Unidad = unidad;
            }
        }

        void InmutableConvert(String unidad) => InmutableConvert(Unidad.Of(unidad));

        void InmutableConvert(int idUnidad) => InmutableConvert(Unidad.Of(idUnidad));

        void InmutableConvert() => InmutableConvert(this.Unidad.UnidadBase());

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
