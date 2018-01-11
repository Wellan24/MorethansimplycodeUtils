using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaParamInsituMuestraAgua
    {
        private static int[] idsMostrarTemperatura = null;
        public static int[] GetIdsMostrarTemperatura()
        {
            if (idsMostrarTemperatura == null)
                idsMostrarTemperatura = PersistenceManager
                    .SelectByProperty<ParamInsituMuestraAgua>(propiedad:"MostrarTemperatura", value:true, columnsToSelect: new string[] { "Id" })
                    .Select(p=>p.Id).ToArray();
            return idsMostrarTemperatura;
        }

        private static int[] idsMostrarCaudal = null;
        public static int[] GetIdsMostrarCaudal()
        {
            if (idsMostrarCaudal == null)
                idsMostrarCaudal = PersistenceManager
                    .SelectByProperty<ParamInsituMuestraAgua>(propiedad: "MostrarCaudal", value: true, columnsToSelect: new string[] { "Id" })
                    .Select(p => p.Id).ToArray();
            return idsMostrarCaudal;
        }

        private static ParamInsituMuestraAgua[] parametros = null;
        public static ParamInsituMuestraAgua[] GetParametros()
        {
            if (parametros == null)
                parametros = PersistenceManager.SelectAll<ParamInsituMuestraAgua>().ToArray();
            return parametros;
        }
    }

    [TableProperties("paraminsitu_muestraagua")]
    public class ParamInsituMuestraAgua : PersistenceData
    {
        [ColumnProperties("id_paraminsitumuestraagua", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("nombre_paraminsitumuestraagua")]
        public String Nombre { get; set; }

        [ColumnProperties("mostrartemperatura_paraminsitumuestraagua")]
        public Boolean MostrarTemperatura { get; set; }

        [ColumnProperties("mostrarcaudal_paraminsitumuestraagua")]
        public Boolean MostrarCaudal { get; set; }
    }
}
