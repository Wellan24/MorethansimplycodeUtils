using Cartif.Collections;
using Cartif.Extensions;
using System;
using System.Diagnostics;

namespace Cartif.Util
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A cartif stopwatch. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class CartifStopwatch : CartifDictionary<String, Stopwatch>
    {
        #region Properties

        private static CartifStopwatch instance;    /* The instance */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the instance. </summary>
        /// <value> The instance. </value>
        ///--------------------------------------------------------------------------------------------------
        public static CartifStopwatch INSTANCE
        {
            get
            {
                if (instance == null)
                    instance = new CartifStopwatch();

                return CartifStopwatch.instance;
            }
            set { CartifStopwatch.instance = value; }
        }

        #endregion

        #region Constructors

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Default constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public CartifStopwatch() : base(2) { }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="capacity"> The capacity. </param>
        ///--------------------------------------------------------------------------------------------------
        public CartifStopwatch(int capacity) : base(capacity) { }

        #endregion

        #region Instance Methods

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Starts the stopwatch identified by id. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="id"> The identifier. </param>
        ///--------------------------------------------------------------------------------------------------
        public void Start(String id)
        {
            Stopwatch sw = new Stopwatch();
            this[id] = sw;
            sw.Start();
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Restarts the stopwatch identified by id. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="id"> The identifier. </param>
        /// <returns> A long. </returns>
        ///--------------------------------------------------------------------------------------------------
        public long Restart(String id)
        {
            Stopwatch sw = this[id];

            if (sw == null)
            {
                sw = new Stopwatch();
                this[id] = sw;
                sw.Start();

                return 0;
            }
            else
            {
                sw.Stop();

                long toReturn = sw.ElapsedMilliseconds;

                sw.Reset();
                sw.Start();

                return toReturn;
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Stops the stopwatch identified by id. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="id"> The identifier. </param>
        /// <returns> A long. </returns>
        ///--------------------------------------------------------------------------------------------------
        public long Stop(String id)
        {
            Stopwatch sw = this[id];
            sw.ThrowIfArgumentIsNull("Stopwatch[id]");

            sw.Stop();

            return sw.ElapsedMilliseconds;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Print the elapsed time of the stopwatch identified by id. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="id">      The identifier. </param>
        /// <param name="stop">    true to stop. </param>
        /// <param name="message"> The message. </param>
        ///--------------------------------------------------------------------------------------------------
        public void PrintElapsedTime(String id, Boolean stop, String message)
        {
            Stopwatch sw = this[id];
            sw.ThrowIfArgumentIsNull("Stopwatch[id]");

            if (stop)
                sw.Stop();

            if (message != null)
                Console.WriteLine("{0} | Stopwatch {1} elapsed: {2}", message, id, sw.Elapsed);
            else
                Console.WriteLine("Stopwatch {0} elapsed: {1}", id, sw.Elapsed);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Converts this CartifStopwatch to a string elapsed time. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="id">      The identifier. </param>
        /// <param name="stop">    true to stop. </param>
        /// <param name="message"> The message. </param>
        /// <returns> The given data converted to a String. </returns>
        ///--------------------------------------------------------------------------------------------------
        public String ToStringElapsedTime(String id, Boolean stop, String message)
        {
            Stopwatch sw = this[id];
            sw.ThrowIfArgumentIsNull("Stopwatch[id]");

            if (stop)
                sw.Stop();

            if (message != null)
                return String.Format("{0} | Stopwatch {1} elapsed: {2}", message, id, sw.Elapsed);
            else
                return String.Format("Stopwatch {0} elapsed: {1}", id, sw.Elapsed);
        }

        #endregion

        #region Class Methods

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Starts a stopwatch. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="id"> The identifier. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void StartStopwatch(String id) { CartifStopwatch.INSTANCE.Start(id); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Restart stopwatch. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="id"> The identifier. </param>
        /// <returns> A long. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static long RestartStopwatch(String id) { return CartifStopwatch.INSTANCE.Restart(id); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Stops a stopwatch. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="id"> The identifier. </param>
        /// <returns> A long. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static long StopStopwatch(String id) { return CartifStopwatch.INSTANCE.Stop(id); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets a stopwatch. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="id"> The identifier. </param>
        /// <returns> The stopwatch. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static Stopwatch GetStopwatch(String id) { return CartifStopwatch.INSTANCE[id]; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Print stopwatch elapsed time. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="id">      The identifier. </param>
        /// <param name="stop">    true to stop. </param>
        /// <param name="message"> The message. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void PrintStopwatchElapsedTime(String id, Boolean stop, String message) { CartifStopwatch.INSTANCE.PrintElapsedTime(id, stop, message); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Print stopwatch elapsed time. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="id">      The identifier. </param>
        /// <param name="stop">    true to stop. </param>
        /// <param name="message"> The message. </param>
        /// <returns> The given data converted to a String. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static String ToStringStopwatchElapsedTime(String id, Boolean stop, String message) { return CartifStopwatch.INSTANCE.ToStringElapsedTime(id, stop, message); }

        #endregion
    }
}
