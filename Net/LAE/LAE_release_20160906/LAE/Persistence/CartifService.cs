using LAE.CartifService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace LAE.AccesoDatos
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary> A cartif service. </summary>
    /// <remarks> Oscvic, 05/02/2016. </remarks>
    ///-------------------------------------------------------------------------------------------------
    public class CartifServices
    {
        /// <summary> The service. </summary>
        private static CartifWebService service = null;
        /// <summary> 5 min. </summary>
        private static Timer cerrarService = new Timer(300000);

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Static constructor. </summary>
        /// <remarks> Oscvic, 05/02/2016. </remarks>
        ///-------------------------------------------------------------------------------------------------
        static CartifServices()
        {
            cerrarService.Elapsed += CerrarService_Elapsed;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the service. </summary>
        /// <value> The service. </value>
        ///-------------------------------------------------------------------------------------------------
        public static CartifWebService Service
        {
            get
            {
                if (service == null || service.State.HasFlag(CommunicationState.Closed | CommunicationState.Closing))
                {
                    service = new CartifWebService(new CartifServiceSoapClient());
                    service.Open();
                }

                return service;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Cerrar service elapsed. </summary>
        /// <remarks> Oscvic, 05/02/2016. </remarks>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Elapsed event information. </param>
        ///-------------------------------------------------------------------------------------------------
        private static void CerrarService_Elapsed(Object sender, ElapsedEventArgs e)
        {
            if (!service.InUse)
                service.Dispose();
        }

    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary> A cartif service. </summary>
    /// <remarks> Oscvic, 05/02/2016. </remarks>
    ///-------------------------------------------------------------------------------------------------
    public class CartifWebService : IDisposable
    {
        /// <summary> The inner service. </summary>
        private CartifServiceSoapClient innerService;

        public Boolean InUse { get; set; } = false;
        public CommunicationState State { get { return innerService.State; } }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 05/02/2016. </remarks>
        /// <param name="service"> The service. </param>
        ///-------------------------------------------------------------------------------------------------
        public CartifWebService(CartifServiceSoapClient service)
        {
            innerService = service;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Login LDAP zimbra. </summary>
        /// <remarks> Oscvic, 05/02/2016. </remarks>
        /// <param name="user"> The user. </param>
        /// <param name="pass"> The pass. </param>
        /// <returns> true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------
        public Boolean LoginLDAPZimbra(String user, String pass)
        {
            InUse = true;
            Boolean toReturn = innerService.LoginLDAPZimbra(user, pass);
            InUse = false;
            return toReturn;
        }

        public void Open()
        {
            innerService.Open();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Realiza tareas definidas por la aplicación asociadas a la liberación o al
        ///           restablecimiento de recursos no administrados. </summary>
        /// <remarks> Oscvic, 05/02/2016. </remarks>
        ///-------------------------------------------------------------------------------------------------
        public void Dispose()
        {
            if (!InUse)
            {
                try
                {
                    innerService.Close();
                }
                catch (CommunicationException)
                {
                    innerService.Abort();
                }
                catch (TimeoutException)
                {
                    innerService.Abort();
                }
                catch (Exception)
                {
                    innerService.Abort();
                    throw;
                }
            }
        }
    }
}
