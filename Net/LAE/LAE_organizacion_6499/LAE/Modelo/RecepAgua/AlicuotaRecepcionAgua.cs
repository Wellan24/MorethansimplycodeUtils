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
    public static class FactoriaAlicuota_recepcionagua
    {
        //TODO DELETE
        public static void LoadData(this AlicuotaRecepcionAgua ali)
        {
            ali.Parametros = new ObservableCollection<LineaAliRecepcionAgua>();
            PersistenceDataManipulation.LoadData("IdAlicuota", ali.Id, ali.Parametros);
        }
    }

    [TableProperties("alicuota_recepcionagua")]
    public class AlicuotaRecepcionAgua : PersistenceData, IModelo
    {
        [ColumnProperties("id_alicuotarecepcionagua", IsId =true, IsAutonumeric =true)]
        public int Id { get; set; }

        [ColumnProperties("numero_alicuotarecepcionagua")]
        public int NumeroAlicuotas { get; set; }

        [ColumnProperties("recvidrio_alicuotarecepcionagua")]
        public Boolean RecipienteVidrio { get; set; }

        [ColumnProperties("cantidad_alicuotarecepcionagua")]
        public int Cantidad { get; set; }

        [ColumnProperties("idudscantidad_alicuotarecepcionagua")]
        public int? IdUdsCantidad { get; set; }

        [ColumnProperties("idmuestra_alicuotarecepcionagua")]
        public int IdMuestra { get; set; }

        public int NumId { get; set; }

        public override bool Equals(object obj)
        {
            return Object.ReferenceEquals(this, obj);
        }

        public ObservableCollection<LineaAliRecepcionAgua> Parametros;
    }
}
