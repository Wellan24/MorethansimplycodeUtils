using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaAlicuotaMuestraAgua
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("alicuota_muestraagua")]
    public class AlicuotaMuestraAgua : PersistenceData,IModelo
    {
        [ColumnProperties("id_alicuotaaguamuestraagua", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("numero_alicuotaaguamuestraagua")]
        public int NumAlicuotas { get; set; }

        [ColumnProperties("recvidrio_alicuotaaguamuestraagua")]
        public Boolean RecipienteVidrio { get; set; }

        [ColumnProperties("tipovidrio_alicuotaaguamuestraagua")]
        public String TipoVidrio { get; set; }

        [ColumnProperties("lavado_alicuotaaguamuestraagua")]
        public String TipoLavado { get; set; }

        [ColumnProperties("refrigeracion_alicuotaaguamuestraagua")]
        public Boolean Refrigeracion { get; set; }

        [ColumnProperties("conservante_alicuotaaguamuestraagua ")]
        public Boolean Conservante { get; set; }

        [ColumnProperties("tecnicaconservacion_alicuotaaguamuestraagua")]
        public String TecnicaConservacion { get; set; }

        [ColumnProperties("idmuestraagua_alicuotaaguamuestraagua")]
        public int IdMuestra { get; set; }

        public override bool Equals(object obj)
        {
            return Object.ReferenceEquals(this, obj);
        }
    }
}
