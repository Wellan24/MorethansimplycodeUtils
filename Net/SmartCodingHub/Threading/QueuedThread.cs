using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace Cartif.Threading
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> Basic implementation of a Thread with a pool, that keeps alive, checking its actions to
    ///           do. By default, it keeps alive 60 segs after all actions done. Resets when other
    ///           action added to the Queue. </summary>
    /// <remarks> Oscvic, 2016-01-04. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class QueuedThread
    {
        private static object lockObjForQueueOperations = new object(); /* The lock object for queue operations */
        private static object lockForQuit = new object();   /* The lock for quit */
        private Queue<QueuedAction> actionsQueue;   /* Queue of actions */
        private Thread thread;  /* The thread */

        public event OnSelected OnSelectedEvent;    /* Event queue for all listeners interested in onSelected events. */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Executes the selected action. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="finishedThread"> The finished thread. </param>
        ///--------------------------------------------------------------------------------------------------
        public delegate void OnSelected(QueuedThread finishedThread);

        private Boolean quit;   /* true to quit */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets a value indicating whether the quit. </summary>
        /// <value> true if quit, false if not. </value>
        ///--------------------------------------------------------------------------------------------------
        public Boolean Quit
        {
            get { return quit; }
            set
            {
                quit = value;
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Default constructor. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public QueuedThread()
        {
            actionsQueue = new Queue<QueuedAction>();
            thread = new Thread(() => MakeActions());
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Adds an action. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="a"> The QueuedAction to process. </param>
        ///--------------------------------------------------------------------------------------------------
        public void AddAction(QueuedAction a)
        {
            if (a != null)
                lock (lockObjForQueueOperations)
                    actionsQueue.Enqueue(a);

            Start();
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the next action. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <returns> The next action. </returns>
        ///--------------------------------------------------------------------------------------------------
        public QueuedAction GetNextAction()
        {
            lock (lockObjForQueueOperations)
            {
                if (actionsQueue.Count > 0)
                    return actionsQueue.Dequeue();
                else
                    return null;
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Starts this QueuedThread. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public void Start()
        {
            if (thread.IsAlive)
            {
                lock (lockForQuit)
                {
                    quit = false;
                    Monitor.Pulse(lockForQuit);
                }
            }
            else
            {
                if (thread != null && thread.ThreadState == ThreadState.Unstarted)
                {
                    quit = false;
                    thread.Start();
                }
                else
                {
                    quit = false;
                    thread = new Thread(() => MakeActions());
                    thread.Start();
                }

            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Determine if we should quit. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <returns> true if it succeeds, false if it fails. </returns>
        ///--------------------------------------------------------------------------------------------------
        private Boolean ShouldQuit()
        {
            lock (lockForQuit)
            {
                if (quit)
                    return true;

                return !Monitor.Wait(lockForQuit, TimeSpan.FromSeconds(60));
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Makes the actions. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        ///--------------------------------------------------------------------------------------------------
        private void MakeActions()
        {
            QueuedAction queuedAction = GetNextAction();
            while (queuedAction != null)
            {
                try
                {
                    queuedAction.ActionToDo.Invoke();
                }
                catch (Exception ex)
                {
                    if (queuedAction.ActionWhenException != null)
                    {
                        try { queuedAction.ActionWhenException.Invoke(ex); }
                        catch (Exception) {/* Empty */ }
                    }
                }
                queuedAction = GetNextAction();
            }

            NotifyWorkFinished();

            if (!ShouldQuit())
                MakeActions();
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Notifies the work finished. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        ///--------------------------------------------------------------------------------------------------
        private void NotifyWorkFinished()
        {
            if (OnSelectedEvent != null)
                OnSelectedEvent(this);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Stops this QueuedThread. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public void Stop()
        {
            lock (lockForQuit)
            {
                quit = true;
                Monitor.Pulse(lockForQuit);
            }
        }
    }
}
