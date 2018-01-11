using Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaFoco_pmatmosfera
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("foco_pmatmosfera")]
    public class FocoAtmosfera : PersistenceData
    {
        [ColumnProperties("id_focopmatmosfera", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("denominacion_focopmatmosfera")]
        public String Denominacion { get; set; }

        [ColumnProperties("descripcion_focopmatmosfera")]
        public String Descripcion { get; set; }

        [ColumnProperties("idregimen_focopmatmosfera")]
        public int? IdRegimen { get; set; }

        [ColumnProperties("depuracion_focopmatmosfera")]
        public Boolean Depuracion { get; set; }

        [ColumnProperties("tecnicadepuracion_focopmatmosfera")]
        public String TecnicaDepuracion { get; set; }

        [ColumnProperties("combustible_focopmatmosfera")]
        public String Combustible { get; set; }

        [ColumnProperties("condiciones_focopmatmosfera")]
        public String Condiciones { get; set; }

        [ColumnProperties("accesos_focopmatmosfera")]
        public String Accesos { get; set; }

        [ColumnProperties("circular_focopmatmosfera")]
        public Boolean? Circular { get; set; }

        [ColumnProperties("diametroconex_focopmatmosfera")]
        public decimal DiametroConex { get; set; }

        [ColumnProperties("idudsdiametroconex_focopmatmosfera")]
        public int? IdUdsDiametroConex { get; set; }

        [ColumnProperties("diamchimenea_focopmatmosfera")]
        public decimal DiametroChimenea { get; set; }

        [ColumnProperties("lado1chimenea_focopmatmosfera")]
        public decimal Lado1Chimenea { get; set; }

        [ColumnProperties("lado2chimenea_focopmatmosfera")]
        public decimal Lado2Chimenea { get; set; }

        [ColumnProperties("idudschimenea_focopmatmosfera")]
        public int? IdUdsChimenea { get; set; }

        [ColumnProperties("perturinferior_focopmatmosfera")]
        public String PerturbacionInferior { get; set; }

        [ColumnProperties("idudsperturinferior_focopmatmosfera")]
        public int? IdUdsPerturbacionInferior { get; set; }

        [ColumnProperties("pertursuperior_focopmatmosfera")]
        public String PerturbacionSuperior { get; set; }

        [ColumnProperties("idudspertursuperior_focopmatmosfera")]
        public int? IdUdsPerturbacionSuperior { get; set; }

        [ColumnProperties("condicionesmuestreo_focopmatmosfera")]
        public String CondicionesMuestreo { get; set; }

        [ColumnProperties("observaciones_focopmatmosfera")]
        public String Observaciones { get; set; }

        [ColumnProperties("idtecnico_focopmatmosfera")]
        public int IdTecnico { get; set; }

        public ObservableCollection<MuestreosFocoAtm> Muestreos;
        public ObservableCollection<ConexionFocoAtm> Conexiones;
    }
}
