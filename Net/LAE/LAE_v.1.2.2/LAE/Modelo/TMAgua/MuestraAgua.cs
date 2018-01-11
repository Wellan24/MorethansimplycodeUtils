using Cartif.Extensions;
using LAE.Clases;
using Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public static class FactoriaMuestraAgua
    {
        public static void LoadData(this MuestraAgua ma)
        {
            PersistenceDataManipulation.LoadData("IdMuestra", ma.Id, ma.Materiales);
            PersistenceDataManipulation.LoadData("IdMuestra", ma.Id, ma.Localizaciones);
            PersistenceDataManipulation.LoadData("IdMuestra", ma.Id, ref ma.TipoMuestra);
            PersistenceDataManipulation.LoadData("IdMuestra", ma.Id, ma.ParametrosInsitu);
            PersistenceDataManipulation.LoadData("IdMuestra", ma.Id, ma.ParametrosLaboratorio);
            PersistenceDataManipulation.LoadData("IdMuestra", ma.Id, ma.Alicuotas);
            PersistenceDataManipulation.LoadData("IdMuestra", ma.Id, ma.Equipos);
        }

        public static void LoadAlicuotas(this MuestraAgua ma)
        {
            ma.Alicuotas = new ObservableCollection<AlicuotaMuestraAgua>();
            PersistenceDataManipulation.LoadData("IdMuestra", ma.Id, ma.Alicuotas);
        }

        public static void LoadParametros(this MuestraAgua ma)
        {
            ma.ParametrosLaboratorio = new ObservableCollection<ParamsLaboratorioMuestraAgua>();
            PersistenceDataManipulation.LoadData("IdMuestra", ma.Id, ma.ParametrosLaboratorio);
        }

        public static void GetMuestraPorPuntoControl(int idPuntoControl, int idTrabajo) { }

    }

    [TableProperties("muestras_agua")]
    public class MuestraAgua : PersistenceData, IModelo
    {
        [ColumnProperties("id_muestraagua", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("descipcion_muestraagua")]
        public String Descripcion { get; set; }

        [ColumnProperties("numcodigo_muestragua")]
        public int NumCodigo { get; set; }

        [ColumnProperties("fecha_muestraagua")]
        public DateTime? Fecha { get; set; }

        [ColumnProperties("hora_muestraagua")]
        public int Hora { get; set; }

        [ColumnProperties("duracion_muestraagua")]
        public int Duracion { get; set; }

        [ColumnProperties("idudsduracion_muestraagua")]
        public int? IdUdsDuracion { get; set; }

        [ColumnProperties("volumen_muestraagua")]
        public int Volumen { get; set; }

        [ColumnProperties("idudsvolumen_muestraagua")]
        public int? IdUdsVolumen { get; set; }

        [ColumnProperties("incidencias_muestraagua")]
        public String Incidencias { get; set; }

        [ColumnProperties("idtecnico_muestraagua")]
        public int IdTecnico { get; set; }

        [ColumnProperties("idpuntocontrol_muestraagua")]
        public int IdPuntoControl { get; set; }

        [ColumnProperties("idtomamuestraagua_muestraagua")]
        public int IdTomaMuestra { get; set; }

        private String getCodigoLae;
        public String GetCodigoLae
        {
            get
            {
                if (getCodigoLae != null)
                    return getCodigoLae + "/" + String.Format("{0:0#}", NumCodigo);
                else
                {
                    TomaMuestraAgua tm = PersistenceManager.SelectByID<TomaMuestraAgua>(IdTomaMuestra);
                    if (tm != null)
                    {
                        Trabajo t = PersistenceManager.SelectByID<Trabajo>(tm.IdTrabajo);
                        Oferta o = PersistenceManager.SelectByID<Oferta>(t.IdOferta);
                        //return o.Codigo + "-TM-" + String.Format("{0:0#}", tm.NumCodigo) + "/" + String.Format("{0:0#}", NumCodigo);
                        return String.Format("{0}-SE-{1:0#}-TM-{2:0#}/{3:0#}", o.Codigo, t.NumCodigo, tm.NumCodigo, NumCodigo);
                    }
                    else
                        return null;
                }


            }
            set
            {
                getCodigoLae = value;
            }
        }

        public ObservableCollection<MaterialesMuestraAgua> Materiales;
        public ObservableCollection<LocalizacionesMuestraAgua> Localizaciones;
        public TiposMuestraMuestraAgua TipoMuestra;
        public ObservableCollection<ParamsInsituMuestraAgua> ParametrosInsitu;
        public ObservableCollection<ParamsLaboratorioMuestraAgua> ParametrosLaboratorio;
        public ObservableCollection<AlicuotaMuestraAgua> Alicuotas;
        public ObservableCollection<EquiposMuestraAgua> Equipos;
    }
}
