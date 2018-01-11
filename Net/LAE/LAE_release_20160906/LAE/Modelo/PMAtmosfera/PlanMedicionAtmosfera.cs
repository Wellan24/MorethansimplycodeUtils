using Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaPlanmedicion_atmosfera
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("planmedicion_atmosfera")]
    public class PlanMedicionAtmosfera : PersistenceData
    {
        [ColumnProperties("id_planmedicionatmosfera", IsId =true, IsAutonumeric =true)]
        public int Id { get; set; }

        [ColumnProperties("objetivo_planmedicionatmosfera")]
        public String Objetivo { get; set; }

        [ColumnProperties("descripcion_planmedicionatmosfera")]
        public String Descripcion { get; set; }

        [ColumnProperties("modificaciones_planmedicionatmosfera")]
        public String Modificaciones { get; set; }

        [ColumnProperties("medidascorreccion_planmedicionatmosfera")]
        public String MedidasCorreccion { get; set; }

        [ColumnProperties("nuevosdefectos_planmedicionatmosfera")]
        public String NuevosDefectos { get; set; }

        [ColumnProperties("observaciones_planmedicionatmosfera")]
        public String Observaciones { get; set; }

        [ColumnProperties("idtecnico_planmedicionatmosfera")]
        public int IdTecnico { get; set; }

        [ColumnProperties("idtrabajo_planmedicionatmosfera")]
        public int IdTrabajo { get; set; }



        public ObservableCollection<EquiposPMAtmosfera> Equipos;
        public ObservableCollection<FechaPMAtmosfera> Fechas;
        public ObservableCollection<PersonalPMAtmosfera> Personal;
    }
}
