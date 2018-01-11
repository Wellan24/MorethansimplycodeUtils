using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaDimensionesPelet
    {
        public static MedicionPNT GetMedicion(int idMuestra)
        {
            return FactoriaMedicionPNT.GetMedicion(idMuestra, "biomasa.dimensiones_pelet", "idmedicion_dimensionespelet");
        }

        public static DimensionesPelet GetParametro(int idMedicion)
        {
            DimensionesPelet tam = PersistenceManager.SelectByProperty<DimensionesPelet>("IdMedicion", idMedicion).FirstOrDefault();
            if (tam != null)
            {
                tam.Clases = PersistenceManager.SelectByProperty<ClasePelet>("IdDimension", tam.Id).ToList();
                foreach (ClasePelet clase in tam.Clases)
                {
                    clase.Longitudes = PersistenceManager.SelectByProperty<LongitudPelet>("IdClase", clase.Id).ToList();
                    clase.Diametros = PersistenceManager.SelectByProperty<DiametroPelet>("IdClase", clase.Id).ToList();
                }
            }
            return tam;
        }

        public static DimensionesPelet GetDefault()
        {
            DimensionesPelet tam = new DimensionesPelet();
            tam.Clases = new List<ClasePelet>();
            ClasePelet clase = new ClasePelet() { };
            clase.Longitudes = new List<LongitudPelet>();
            clase.Diametros = new List<DiametroPelet>();
            tam.Clases.Add(clase);

            return tam;
        }
    }

    [TableProperties("biomasa.dimensiones_pelet")]
    public class DimensionesPelet : PersistenceData, IModelo
    {
        [ColumnProperties("id_dimensionespelet", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("idvprocedimiento_dimensionespelet")]
        public int IdVProcedimiento { get; set; }

        [ColumnProperties("idmedicion_dimensionespelet")]
        public int IdMedicion { get; set; }

        public int IdParametro { get { return 2; } } /* en la tabla ParametroProcedimiento su valor es 2*/

        public List<ClasePelet> Clases;
    }
}
