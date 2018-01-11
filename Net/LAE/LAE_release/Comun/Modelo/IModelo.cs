using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Comun.Modelo
{
    public interface IModelo
    {
        int Id { get; set; }
    }

    public interface ITipoReplicas
    {
    }

    public interface IReplicas<T> where T : ITipoReplicas
    {
        List<T> Replicas { get; set; }
    }

    public interface IMedicion
    {
        void BorrarMedicion();
    }
}
