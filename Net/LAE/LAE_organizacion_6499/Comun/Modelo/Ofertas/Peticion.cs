using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Comun.Modelo
{
    public static class FactoriaPeticiones
    {
        public static void LoadPuntosControl(this Peticion pet)
        {
            pet.PuntosControl = PersistenceManager.SelectByProperty<PuntocontrolPeticion>("IdPeticion", pet.Id).ToArray();
        }
    }

    [TableProperties("peticiones")]
    public class Peticion : PersistenceData
    {
        [ColumnProperties("id_peticion", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("fecha_peticion")]
        public DateTime? Fecha { get; set; }

        [ColumnProperties("requieretomamuestra_peticion")]
        public Boolean? RequiereTomaMuestra { get; set; }

        [ColumnProperties("lugarmuestra_peticion")]
        public String LugarMuestra { get; set; }

        [ColumnProperties("numpuntosmuestreo_peticion")]
        public int? NumPuntosMuestreo { get; set; }

        [ColumnProperties("trabajopuntual_peticion")]
        public Boolean? TrabajoPuntual { get; set; }

        [ColumnProperties("frecuencia_peticion")]
        public String Frecuencia { get; set; }

        [ColumnProperties("plazorealizacion_peticion")]
        public String PlazoRealizacion { get; set; }

        [ColumnProperties("observaciones_peticion")]
        public String Observaciones { get; set; }

        [ColumnProperties("idcliente_peticion")]
        public int IdCliente { get; set; }

        [ColumnProperties("idcontacto_peticion")]
        public int IdContacto { get; set; }

        [ColumnProperties("idtecnico_peticion")]
        public int IdTecnico { get; set; }

        [ColumnProperties("idoferta_peticion")]
        public int? IdOferta { get; set; }

        public PuntocontrolPeticion[] PuntosControl { get; set; }
    }
}
