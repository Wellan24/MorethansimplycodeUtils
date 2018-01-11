using Cartif.Extensions;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaUnidades
    {
        private static Unidad[] unidades = null;
        public static Unidad[] GetUnidades()
        {
            if (unidades == null)
                unidades = PersistenceManager.SelectAll<Unidad>().ToArray();
            return unidades;
        }

        private static Unidad[] unidadesTipo = null;
        private static String tipoU = null;
        public static Unidad[] GetUnidadesByTipo(String tipo)
        {
            if (unidadesTipo == null || tipoU == null || !tipoU.Equals(tipo))
            {
                TipoUnidad tipoUnidad = PersistenceManager.SelectByProperty<TipoUnidad>("Nombre", tipo).FirstOrDefault();
                unidadesTipo = PersistenceManager.SelectByProperty<Unidad>("IdTipo", tipoUnidad.Id).ToArray();
            }
            return unidadesTipo;
        }
    }

    [TableProperties("unidades")]
    public partial class Unidad : PersistenceData
    {
        [ColumnProperties("id_unidad", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("nombre_unidad")]
        public String Nombre { get; set; }

        [ColumnProperties("abreviatura_unidad")]
        public String Abreviatura { get; set; }

        [ColumnProperties("factorconversion_unidad")]
        public double FactorConversion { get; set; }

        [ColumnProperties("idtipo_unidad")]
        public int IdTipo { get; set; }

        public TipoUnidad Tipo { get; set; }
    }

    public partial class Unidad
    {
        private static Dictionary<int, Unidad> unidadesId;
        private static Dictionary<String, Unidad> unidadesName;

        static Unidad()
        {
            Dictionary<int, TipoUnidad> tipos = PersistenceManager.SelectAll<TipoUnidad>().ToDictionary(t => t.Id);

            unidadesId = PersistenceManager.SelectAll<Unidad>().Map(u =>
            {
                u.Tipo = tipos[u.IdTipo];
                return u;
            }).ToDictionary(u => u.Id);

            unidadesName = PersistenceManager.SelectAll<Unidad>().Map(u=>
            {
                u.Tipo = tipos[u.IdTipo];
                return u;                
            }).ToDictionary(u=>u.Nombre);

        }

        public static Unidad Of(String nombre)
        {
            return unidadesName[nombre];
        }

        public static Unidad Of(int id)
        {
            return unidadesId[id];
        }

        public Unidad BaseOf()
        {
            return unidadesId.Where(u => u.Value.IdTipo == IdTipo && u.Value.FactorConversion == 1).Select(u => u.Value).FirstOrDefault();
        }
    }
}
