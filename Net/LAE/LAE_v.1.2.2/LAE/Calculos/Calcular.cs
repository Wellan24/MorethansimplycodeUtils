using Cartif.Extensions;
using LAE.Modelo;
using Persistence;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LAE.Calculos
{
    class Calcular
    {
        public static RegresionLineal Pendiente(KeyValuePair<double, double>[] puntos)
        {
            double sumaProductos = 0;
            double sumaX = 0;
            double sumaY = 0;
            double sumaX2 = 0;
            int length = puntos.Length;

            for (int i = 0; i < length; i++)
            {
                var punto = puntos[i];
                sumaProductos += punto.Key * punto.Value;
                sumaX += punto.Key;
                sumaY += punto.Value;
                sumaX2 += Math.Pow(punto.Key, 2);
            }

            double pendiente = (length * sumaProductos - sumaX * sumaY) / (length * sumaX2 - Math.Pow(sumaX, 2));
            double interseccion = (sumaY - pendiente * sumaX) / length;

            return new RegresionLineal() { Pendiente = pendiente, Interseccion = interseccion };
        }

        public static Valor Caudal(Valor volumen, Valor tiempo)
        {
            Valor.Conversor.BatchConvert(volumen, tiempo);
            return Valor.Of(volumen.Value / tiempo.Value, "l/s");
        }

        public static Valor HumedadTotal_8_1(Valor m1, Valor m2, Valor m3, Valor m4, Valor m5)
        {
            Valor.Conversor.BatchConvert("Gramos", m1, m2, m3, m4, m5);
            return Valor.Of((m2.Value - m3.Value - m4.Value + m5.Value) * 100 / (m2.Value - m1.Value), "%");
        }

        public static Valor Densidad_8_2(Valor vol1, Valor m1, Valor m2)
        {
            Valor.Conversor.BatchConvert("Gramos", m1, m2);
            Valor.Conversor.BatchConvert("cm3", vol1);
            return Valor.Of((m2.Value - m1.Value) * 1000 / vol1.Value, "kg/m3");
        }

        public static Valor Finos_8_9(Valor m1, Valor m2)
        {
            Valor.Conversor.BatchConvert("Gramos", m1, m2);
            return Valor.Of((m1.Value - m2.Value) * 100 / m1.Value, "%");
        }

        public static Valor Durabilidad_8_9(Valor m2, Valor m3)
        {
            Valor.Conversor.BatchConvert("Gramos", m2, m3);
            return Valor.Of(m3.Value * 100 / m2.Value, "%");
        }

        public static Valor Humedad3_8_11(Valor m1, Valor m2, Valor m3)
        {
            Valor.Conversor.BatchConvert("Gramos", m1, m2, m3);
            return Valor.Of((m2.Value - m3.Value) * 100 / (m2.Value - m1.Value), "%");
        }

        public static Valor Cenizas_8_3(Valor m1, Valor m2, Valor m3)
        {
            Valor.Conversor.BatchConvert("Gramos", m1, m2, m3);
            return Valor.Of((m3.Value - m1.Value) * 100 / (m2.Value - m1.Value), "%");
        }

        public static Valor SCl_8_8(Valor m1, Valor vol, Valor muestra, Valor blanco, String parametro)
        {
            Valor.Conversor.BatchConvert("Gramos", m1);
            Valor.Conversor.BatchConvert("mg/l", muestra, blanco);
            Valor.Conversor.BatchConvert("Litros", vol);
            double resultado = ((muestra.Value - blanco.Value) * vol.Value / (m1.Value * 1000)) * 100;
            if (parametro.Equals("S"))
                resultado = resultado * 0.3338;
            return Valor.Of(resultado, "%");
        }

        public static Valor Promedio(Unidad unidad, params Valor[] valores)
        {
            if (unidad != null)
                Valor.Conversor.BatchConvert(unidad, valores);
            return Promedio(valores);
        }

        public static Valor Promedio(String unidad, params Valor[] valores)
        {
            if (unidad != null)
                Valor.Conversor.BatchConvert(unidad, valores);
            return Promedio(valores);
        }

        public static Valor Promedio(int idUnidad, params Valor[] valores)
        {
            if (idUnidad != 0)
                Valor.Conversor.BatchConvert(idUnidad, valores);
            return Promedio(valores);
        }

        public static Valor Promedio(params Valor[] valores)
        {
            if (valores != null && valores.Count() > 0)
                return Valor.Of(valores.Average(v => v.Value), valores[0].Unidad);
            return Valor.Of(0, "");
        }

        public static Valor DesviacionEstandar(params Valor[] valores)
        {
            if (valores != null && valores.Count() > 1)
            {
                double average = valores.Average(v => v.Value);
                double sumOfSquaresOfDifferences = valores.Sum(v => Math.Pow(v.Value - average, 2));
                return Valor.Of(Math.Sqrt(sumOfSquaresOfDifferences / (valores.Count() - 1)), valores[0].Unidad);
            }
            return Valor.Of(0, "");
        }

        public static Valor CoeficienteVariacion(params Valor[] valores)
        {
            return Valor.Of(DesviacionEstandar(valores).Value * 100 / Promedio(valores).Value, "");
        }

        public static Valor DiferenciaAbsolutaMaxima(params Valor[] valores)
        {
            return Valor.Of((Maximo(valores).Value - Minimo(valores).Value) / Minimo(valores).Value, "");
        }

        public static Valor DiferenciaAbsoluta(params Valor[] valores)
        {
            return Valor.Of((Maximo(valores).Value - Minimo(valores).Value), valores[0].Unidad);
        }

        public static Valor Maximo(params Valor[] valores)
        {
            return Valor.Of(valores.Max(v => v.Value), valores[0].Unidad);
        }

        public static Valor Minimo(params Valor[] valores)
        {
            return Valor.Of(valores.Min(v => v.Value), valores[0].Unidad);
        }

        public static bool EsAceptado(double value, int idVProcedimiento, int idParametro, double? limite = null)
        {
            double? rangoSuperior = FactoriaAceptacion.GetRango(idVProcedimiento, idParametro, true, limite);
            double? rangoInferior = FactoriaAceptacion.GetRango(idVProcedimiento, idParametro, false, limite);
            return (rangoSuperior != null ? value < rangoSuperior : true) && (rangoInferior != null ? value > rangoInferior : true);
        }

        /// <summary>
        /// Calcula valor e incertidumbre para el informe en función de la versión del procedimiento
        /// </summary>
        /// <param name="value">Resultado de los calculos</param>
        /// <param name="idVProcedimiento">Id de la versión del procedimiento</param>
        /// <param name="idParametro">Id del parámetro</param>
        /// <param name="numDecimal">Número de decimales del valor</param>
        /// <param name="numDecimalIncert">Número de decimales de la incertidumbre</param>
        /// <returns>Devuelve un objeto ResultadoInforme</returns>
        public static ResultadoInforme Resultado(double value, int idVProcedimiento, int idParametro, int? numDecimal = null, int? numDecimalIncert = null)
        {
            ResultadoInforme resultado = new ResultadoInforme();

            double? rangoSuperior = FactoriaAlcance.GetRango(idVProcedimiento, idParametro, true);
            double? rangoInferior = FactoriaAlcance.GetRango(idVProcedimiento, idParametro, false);
            RegresionLineal incertidumbre = FactoriaIncertidumbre.GetIncertidumbre(idVProcedimiento, idParametro, value);

            if (rangoInferior != null && rangoInferior > value)
            {
                resultado.Incertidumbre = null;
                resultado.Valor = "<" + VisualizeDecimals(rangoInferior, numDecimal ?? 0);
            }
            else if (rangoSuperior != null && rangoSuperior < value)
            {
                resultado.Incertidumbre = null;
                resultado.Valor = ">" + VisualizeDecimals(rangoSuperior, numDecimal ?? 0);
            }
            else
            {
                if (numDecimal != null)
                    resultado.Valor = VisualizeDecimals(value, numDecimal ?? 0);
                else
                    resultado.Valor = value.ToString();


                if (incertidumbre != null)
                {
                    if (numDecimalIncert != null)
                        resultado.Incertidumbre = VisualizeDecimals(Redondear(incertidumbre.GetY(value), numDecimalIncert ?? 0, true), numDecimalIncert ?? 0);
                    else
                        resultado.Incertidumbre = incertidumbre.GetY(value).ToString();
                }
            }
            resultado.IdVProcedimiento = idVProcedimiento;
            resultado.IdParametro = idParametro;

            if (incertidumbre.Pendiente == 0 || incertidumbre.Pendiente == null)
                resultado.RangoIncertidumbre = "\u00B1" + incertidumbre.Interseccion;
            else if (incertidumbre.Interseccion == 0 || incertidumbre.Interseccion == null)
                resultado.RangoIncertidumbre = "\u00B1 Valor*" + incertidumbre.Pendiente;
            else
                resultado.RangoIncertidumbre = "\u00B1 (Valor*" + incertidumbre.Pendiente + "+" + incertidumbre.Interseccion + ")";


            if (rangoInferior != null && rangoSuperior != null)
                resultado.Alcance = rangoInferior + "-" + rangoSuperior;
            else if (rangoSuperior != null)
                resultado.Alcance = "<" + rangoSuperior;
            else if (rangoInferior != null)
                resultado.Alcance = ">" + rangoInferior;
            else
                resultado.Alcance = "--";
            return resultado;
        }


        public static double Redondear(double value, int numDecimal, bool? arriba = null)
        {
            if (arriba == true)
                return Math.Ceiling(value * Math.Pow(10, numDecimal)) / Math.Pow(10, numDecimal);
            else if (arriba == false)
                return Math.Floor(value * Math.Pow(10, numDecimal)) / Math.Pow(10, numDecimal);
            else
                return Math.Round(value * Math.Pow(10, numDecimal)) / Math.Pow(10, numDecimal);
        }

        public static string VisualizeDecimals(double? value, int numDecimal)
        {
            if (value == null)
                return String.Empty;


            if (numDecimal >= 0)
            {
                String pattern = "F" + numDecimal;
                return (value ?? 0).ToString(pattern, CultureInfo.InvariantCulture);
            }
            return (Math.Round((value ?? 0) / (10.0 * numDecimal)) * (10.0 * numDecimal)).ToString(CultureInfo.InvariantCulture);

            //return (value ?? 0).ToString(CultureInfo.InvariantCulture);
        }

        public static bool ExisteNull(params Valor[] valores)
        {
            foreach (Valor v in valores)
            {
                if (v == null)
                    return true;
            }
            return false;
        }

    }

    public class ResultadoInforme
    {
        public String Valor { get; set; }
        private String incertidumbre;
        public String Incertidumbre
        {
            get
            {
                if (incertidumbre == null || incertidumbre.Equals(String.Empty))
                    return "--";
                return "\u00B1" + incertidumbre;
            }
            set { incertidumbre = value?.Replace("-", "")?.Replace("\u00B1", ""); }
        }
        public int IdVProcedimiento { get; set; }
        public int IdParametro { get; set; }

        /*Para el Tooltip del Control*/
        public String RangoIncertidumbre { get; set; }
        public String Alcance { get; set; }
    }

    public class RegresionLineal
    {
        public double? Pendiente { get; set; }
        public double? Interseccion { get; set; }

        public RegresionLineal()
        {

        }

        public double GetY(double x)
        {
            return Interseccion ?? 0 + x * Pendiente ?? 0;
        }
    }


}
