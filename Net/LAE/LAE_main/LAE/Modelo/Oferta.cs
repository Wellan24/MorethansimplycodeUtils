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

        [ColumnProperties("numcodigo_oferta")]
        public int NumCodigoOferta { get; set; }

        [ColumnProperties("anno_oferta")]
        public int AnnoOferta { get; set; }

        [ColumnProperties("anulada_oferta")]
        public bool Anulada { get; set; }

        [ColumnProperties("idcliente_oferta")]
        public int IdCliente { get; set; }

        [ColumnProperties("idcontacto_oferta")]
        public int IdContacto { get; set; }

        [ColumnProperties("idtecnico_oferta")]
        public int IdTecnico { get; set; }

        public String Codigo
        {
            get { return String.Format("O-LAE-{0}-{1:00#}", (AnnoOferta - (AnnoOferta / 100) * 100), NumCodigoOferta); }
            set { }
        }
    }
}
