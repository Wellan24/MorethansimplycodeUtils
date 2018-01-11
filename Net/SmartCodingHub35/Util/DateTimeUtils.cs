using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cartif.Extensions;

namespace Cartif.Util
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> -----------------------------------------------------------------
    ///             Namespace:      Cartif.Util Class:          DateTimeUtils Description:    Returns
    ///             the number of weekdays between two dates. Author:         Oscar - Cartif       Date:
    ///             14-10-2015 Notes:          Source:
    ///             http://stackoverflow.com/questions/1820173/calculate-the-number-of-weekdays-between-
    ///             two-dates-in-c-sharp Revision History: Name:           Date:        Description:
    ///           -----------------------------------------------------------------. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public static class DateTimeUtils
    {        
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Week days. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="dtmStart"> The dtm start Date/Time. </param>
        /// <param name="dtmEnd">   The dtm end Date/Time. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int WeekDays(DateTime dtmStart, DateTime dtmEnd)
        {
            if (dtmStart > dtmEnd)
            {
                DateTime temp = dtmStart;
                dtmStart = dtmEnd;
                dtmEnd = temp;
            }

            /* Move border dates to the monday of the first full week and sunday of the last week */
            DateTime startMonday = dtmStart;
            int startDays = 1;
            while (startMonday.DayOfWeek != DayOfWeek.Monday)
            {
                if (startMonday.DayOfWeek != DayOfWeek.Saturday && startMonday.DayOfWeek != DayOfWeek.Sunday)
                {
                    startDays++;
                }
                startMonday = startMonday.AddDays(1);
            }

            DateTime endSunday = dtmEnd;
            int endDays = 0;
            while (endSunday.DayOfWeek != DayOfWeek.Sunday)
            {
                if (endSunday.DayOfWeek != DayOfWeek.Saturday && endSunday.DayOfWeek != DayOfWeek.Sunday)
                {
                    endDays++;
                }
                endSunday = endSunday.AddDays(1);
            }

            int weekDays;

            /* calculate weeks between full week border dates and fix the offset created by moving the border dates */
            weekDays = (Math.Max(0, (int)Math.Ceiling((endSunday - startMonday).TotalDays + 1)) / 7 * 5) + startDays - endDays;

            if (dtmEnd.DayOfWeek == DayOfWeek.Saturday || dtmEnd.DayOfWeek == DayOfWeek.Sunday)
            {
                weekDays -= 1;
            }

            return weekDays;

        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Fines de semana. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="fechaInicio"> The fecha inicio Date/Time. </param>
        /// <param name="fechaFin">    The fecha fin Date/Time. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int FinesDeSemana(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio.Equals(fechaFin))
                return (fechaInicio.DayOfWeek == DayOfWeek.Sunday || fechaInicio.DayOfWeek == DayOfWeek.Saturday) ? 1 : 0;

            int fines = 0;

            int diasEntreFechasDelPeriodo = (fechaFin - fechaInicio).Days;
            fines += diasEntreFechasDelPeriodo / 7 * 2;

            int diaInicioSemana = (int)fechaInicio.DayOfWeekTurbo();
            int diaFinSemana = (int)fechaFin.DayOfWeekTurbo();

            if (diaFinSemana < diaInicioSemana)
                diaFinSemana += 7;

            /* Si hay un sabado entre medias */
            if (diaInicioSemana <= 6 && diaFinSemana >= 6)
                fines++;
            /* si empieza en domingo y va hasta el sábado da 13 días, en 13 días hay 2 sábados */
            if (diaInicioSemana <= 13 && diaFinSemana >= 13)
                fines++;
            /* Si hay un domingo entre medias */
            if (diaInicioSemana <= 7 && diaFinSemana >= 7)
                fines++;

            return fines;
        }
    }
}