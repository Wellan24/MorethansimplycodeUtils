using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;


namespace Cartif.EasyDatabase
{
    public static class EasyExtensions
    {
        public static TReturn Get<TReturn>(this DbDataReader rdr, int pos)
        {
            Object obj = rdr.GetValue(pos);
            if (obj != null && obj.GetType().Equals(typeof(TReturn)))
                return (TReturn)obj;

            return default(TReturn);
        }
    }
}
