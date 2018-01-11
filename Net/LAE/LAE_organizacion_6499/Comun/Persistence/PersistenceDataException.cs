using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Comun.Persistence
{
    public class PersistenceDataException : Exception
    {
        public PersistenceDataException(String message) : base(message) { }
    }
}
