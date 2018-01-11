using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Cartif.Threading
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> An static class to use a QueuedThreadPool. This contains one instance of a pool and
    ///           delegates to it. </summary>
    /// <remarks> Oscvic, 2016-01-04. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public static class DefaultQueuedThreadPool
    {
        private static QueuedThreadPool pool;   /* The pool */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets or Sets the PooledThreads of the pool. If the pool is not started, returns 0. </summary>
        /// <value> The pooled threads. </value>
        ///--------------------------------------------------------------------------------------------------
        public static int PooledThreads
        {
            get { if (pool == null) return 0; return pool.PooledThreads; }
            set { if (pool != null) pool.PooledThreads = value; }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Add an action to the execute queue of the pool of threads. This action will execute, but
        ///           maybe not instantly. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="actionToDo"> The action to do. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void AddAction(Action actionToDo) { StartPool(); pool.AddAction(actionToDo); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Add an action to the execute queue of the pool of threads. This action will execute, but
        ///           maybe not instantly. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="actionToDo"> The action to do. </param>
        /// <param name="exception">  The action to do when a exception occurs in the actionToDo. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void AddActionWithExceptions(Action actionToDo, Action<Exception> exception) { StartPool(); pool.AddActionWithExceptions(actionToDo, exception); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Stops one thread and remove it from the pool, setting the PooledThreads to PooledThreds -
        ///           1. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public static void StopOneThread() { StartPool(); pool.StopOneThread(); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Stops one thread and remove it from the pool, setting the PooledThreads to 0. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public static void StopThreads() { if (pool != null) pool.StopThreads(); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Starts the pool starting the default PooledThreads. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public static void StartPool() { if (pool == null) pool = new QueuedThreadPool(); }
    }
}
