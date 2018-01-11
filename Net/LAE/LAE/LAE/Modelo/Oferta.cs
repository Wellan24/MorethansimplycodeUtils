using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaOfertas
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("ofertas")]
    public class Oferta : PersistenceData
    {
        [ColumnProperties("id_oferta", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("codigo_oferta")]
        public int CodigoOferta { get; set; }

        [ColumnProperties("anno_oferta")]
        public int AnnoOferta { get; set; }

        [ColumnProperties("idcliente_oferta")]
        public int IdCliente { get; set; }

        [ColumnProperties("idcontacto_oferta")]
        public int IdContacto { get; set; }

        [ColumnProperties("idtecnico_oferta")]
        public int IdTecnico { get; set; }

    }
}
