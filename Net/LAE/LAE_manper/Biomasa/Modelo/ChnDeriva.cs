using LAE.Comun.Modelo;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Biomasa.Modelo
{
    public class FactoriaChnDeriva
    {
        public static ChnDeriva GetCHNderiva(int idEnsayo)
        {
            ChnDeriva chn = PersistenceManager.SelectByProperty<ChnDeriva>("IdEnsayo", idEnsayo).FirstOrDefault();
            if (chn != null)
                chn.Replicas = PersistenceManager.SelectByProperty<ReplicaChnDeriva>("IdCHNderiva", chn.Id).ToList();

            return chn;
        }

        public static ChnDeriva GetDefault(int idEnsayo)
        {
            int idGramos = Unidad.Of("Gramos").Id;
            int idPorc = Unidad.Of("%").Id;
            ChnDeriva chn = new ChnDeriva() { IdEnsayo=idEnsayo, BlancoC = false, BlancoH = false, BlancoN = false, ValorDerivaC=false, ValorDerivaH=false, ValorDerivaN=false };
            chn.Replicas = new List<ReplicaChnDeriva>();
            chn.Replicas.Add(new ReplicaChnDeriva() { Valido=true, IdUdsMasaC = idGramos, IdUdsMasaH = idGramos, IdUdsMasaN = idGramos, IdUdsValorC = idPorc, IdUdsValorH = idPorc, IdUdsValorN = idPorc });
            chn.Replicas.Add(new ReplicaChnDeriva() { Valido=true, IdUdsMasaC = idGramos, IdUdsMasaH = idGramos, IdUdsMasaN = idGramos, IdUdsValorC = idPorc, IdUdsValorH = idPorc, IdUdsValorN = idPorc });
            chn.Replicas.Add(new ReplicaChnDeriva() { Valido=true, IdUdsMasaC = idGramos, IdUdsMasaH = idGramos, IdUdsMasaN = idGramos, IdUdsValorC = idPorc, IdUdsValorH = idPorc, IdUdsValorN = idPorc });

            return chn;
        }
    }

    [TableProperties("biomasa.chn_deriva")]
    public class ChnDeriva : PersistenceData, IModelo
    {
        [ColumnProperties("id_chnderiva", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("blancoc_chnderiva")]
        public Boolean? BlancoC { get; set; }

        [ColumnProperties("blancoh_chnderiva")]
        public Boolean? BlancoH { get; set; }

        [ColumnProperties("blancon_chnderiva")]
        public Boolean? BlancoN { get; set; }

        [ColumnProperties("valorderivac_chnderiva")]
        public Boolean? ValorDerivaC { get; set; }

        [ColumnProperties("valorderivah_chnderiva")]
        public Boolean? ValorDerivaH { get; set; }

        [ColumnProperties("valorderivan_chnderiva")]
        public Boolean? ValorDerivaN { get; set; }

        [ColumnProperties("idtecnico_chnderiva")]
        public int IdTecnico { get; set; }

        [ColumnProperties("observaciones_chnderiva")]
        public String Observaciones { get; set; }

        [ColumnProperties("idvprocedimiento_chnderiva")]
        public int IdVProcedimiento { get; set; }

        [ColumnProperties("idmaterialreferencia_chnderiva")]
        public int IdMaterialReferencia { get; set; }

        [ColumnProperties("idensayo_chnderiva")]
        public int IdEnsayo { get; set; }

        public List<ReplicaChnDeriva> Replicas { get; set; }
    }
}