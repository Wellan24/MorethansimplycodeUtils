///-------------------------------------------------------------------------------------------------
// file:	Cartif\Extensions\CommunicationObjectExtensions.cs
//
// summary:	Implements the communication object extensions class
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Cartif.Extensions
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary> A communication object extensions. </summary>
    ///
    /// <remarks> Oscvic, 05/02/2016. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public static class CommunicationObjectExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Called when the instance is disposed. </summary>
        ///
        /// <remarks> Oscvic, 05/02/2016. </remarks>
        ///
        /// <param name="conn"> The conn to act on. </param>
        ///-------------------------------------------------------------------------------------------------

        public static void Dispose(this ICommunicationObject conn)
        {
            conn.ThrowIfArgumentIsNull("El objeto de conexión no puede ser null.");

            try
            {
                conn.Close();
            }
            catch (CommunicationException)
            {
                conn.Abort();
            }
            catch (TimeoutException)
            {
                conn.Abort();
            }
            catch (Exception)
            {
                conn.Abort();
                throw;
            }
        }

    }
}
