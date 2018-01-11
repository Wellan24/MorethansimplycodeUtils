using LAE.Clases;
using LAE.Comun.Modelo;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public static class FactoriaMuestra_recepcionagua
    {
        public static void LoadData(this MuestraRecepcionAgua ma)
        {
            PersistenceDataManipulation.LoadData("IdMuestra", ma.Id, ma.Alicuotas);
            if (ma.Alicuotas != null)
            {
                foreach (AlicuotaRecepcionAgua item in ma.Alicuotas)
                {
                    PersistenceDataManipulation.LoadData("IdAlicuota", item.Id, ma.Parametros);
                }
            }
            
        }
    }

    [TableProperties("muestra_recepcionagua")]
    public class MuestraRecepcionAgua : PersistenceData, IModelo
    {
        [ColumnProperties("id_muestrarecepcionagua", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("codtomamuestra_muestrarecepcionagua")]
        public String CodigoToma { get; set; }

        [ColumnProperties("numcodigo_muestrarecepcionagua")]
        public int CodigoLae { get; set; }

        [ColumnProperties("descripcion_muestrarecepcionagua")]
        public String Descripcion { get; set; }

        [ColumnProperties("tienetomamuestra_muestrarecepcionagua")]
        public Boolean TieneTomaMuestra { get; set; }

        [ColumnProperties("idpuntocontrol_muestrarecepcionagua")]
        public int IdPuntoControl { get; set; }

        [ColumnProperties("idrecepcion_muestrarecepcionagua")]
        public int IdRecepcion { get; set; }

        public ObservableCollection<AlicuotaRecepcionAgua> Alicuotas;
        public ObservableCollection<LineaAliRecepcionAgua> Parametros;

        public String GetCodigoLae
        {
            get {
                RecepcionAgua rec = PersistenceManager.SelectByID<RecepcionAgua>(IdRecepcion);
                if (rec != null)
                {
                    Trabajo t = PersistenceManager.SelectByID<Trabajo>(rec.IdTrabajo);
                    Oferta o = PersistenceManager.SelectByID<Oferta>(t.IdOferta);
                    return String.Format("{0}-SE-{1:0#}-M-1{2:000#}-{3:yy}", o.Codigo, t.NumCodigo, CodigoLae, rec.FechaRecepcion);
                    //return o.Codigo + String.Format("-M-1{0:000#}-{1:yy}", CodigoLae, rec.FechaRecepcion);
                }
                else
                    return null;
            }
            set { }
        }
    }
}
