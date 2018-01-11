using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cartif.Extensions;
using System.Collections.ObjectModel;
using LAE.Comun.Modelo;

namespace LAE.Modelo
{
    public class FactoriaIPuntoControl
    {
        public static IPuntoControl GetIPuntoControl(PuntocontrolRevision pc, bool withId = false)
        {
            //return new IPuntoControl
            //{
            //    Nombre = pc.Nombre,
            //    Importe = pc.Importe,
            //    Observaciones = pc.Observaciones,
            //    IdRelacion = pc.IdRevision,
            //    Lineas = new ObservableCollection<ILineasParametros>(pc.LineasRevision
            //        .Map(l => new ILineasParametros { Cantidad = l.Cantidad, IdParametro = l.IdParametro, IdRelacion = l.IdPControlRevisionOferta }))
            //};

            ILineasParametros ilp;

            IPuntoControl ipc = new IPuntoControl
            {
                Nombre = pc.Nombre,
                Importe = pc.Importe,
                Observaciones = pc.Observaciones,
                IdRelacion = pc.IdRevision,
                Lineas = new ObservableCollection<ILineasParametros>(pc.LineasRevision
                    .Map(l =>
                    {
                        ilp = new ILineasParametros { Cantidad = l.Cantidad, IdParametro = l.IdParametro, IdRelacion = l.IdPControlRevisionOferta };
                        if (withId)
                            ilp.Id = l.Id;
                        return ilp;
                    }))
            };
            if (withId)
                ipc.Id = pc.Id;

            return ipc;
        }

        public static IPuntoControl GetIPuntoControl(PuntocontrolPeticion pc)
        {
            return new IPuntoControl
            {
                Nombre = pc.Nombre,
                Importe = pc.Importe,
                Observaciones = pc.Observaciones,
                IdRelacion = pc.IdPeticion,
                Lineas = new ObservableCollection<ILineasParametros>(pc.LineasPeticion
                    .Map(l => new ILineasParametros { Cantidad = l.Cantidad, IdParametro = l.IdParametro, IdRelacion = l.IdPControlPeticion }))
            };
        }
    }

    public class IPuntoControl
    {
        public int Id { get; set; }
        public String Nombre { get; set; }
        public String Observaciones { get; set; }
        public decimal Importe { get; set; }
        public int IdRelacion { get; set; }
        public ObservableCollection<ILineasParametros> Lineas { get; set; }

        public IPuntoControl()
        {
            Lineas = new ObservableCollection<ILineasParametros>() { new ILineasParametros() };
        }
    }
}
