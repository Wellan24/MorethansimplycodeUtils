using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace Cartif.Threading
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A thread pool to add Actions and Actions that can throw exceptions. The exception
    ///           treatment is based on Erlang Thread engine. </summary>
    /// <remarks> Oscvic, 2016-01-04. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class QueuedThreadPool
    {
        private static object lockForFinish = new object(); /* The lock for finish */

        private Queue<QueuedThread> waitingThreads; /* The waiting threads */
        private LinkedList<QueuedThread> workingThreads;    /* The working threads */

        private int pooledThreads;  /* The pooled threads */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the pooled threads. </summary>
        /// <value> The pooled threads. </value>
        ///--------------------------------------------------------------------------------------------------
        public int PooledThreads
        {
            get { return pooledThreads; }
            set { SetPooledThreads(value); }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Default constructor. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public QueuedThreadPool()
        {
            /* Default settings */
            pooledThreads = 1;
            waitingThreads = new Queue<QueuedThread>(pooledThreads);
            workingThreads = new LinkedList<QueuedThread>();

            /* Init the default Threads */
            for (int i = 0; i < pooledThreads; i++)
                waitingThreads.Enqueue(CreateNewThread());

            Application.ApplicationExit += (s, e) => StopThreads();
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Adds an action. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="actionToDo"> The action to do. </param>
        ///--------------------------------------------------------------------------------------------------
        public void AddAction(Action actionToDo)
        {
            GetNextThread().AddAction(new QueuedAction() { ActionToDo = actionToDo });
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Adds an action with exceptions to 'exception'. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="actionToDo"> The action to do. </param>
        /// <param name="exception">  The exception. </param>
        ///--------------------------------------------------------------------------------------------------
        public void AddActionWithExceptions(Action actionToDo, Action<Exception> exception)
        {
            GetNextThread().AddAction(new QueuedAction() { ActionToDo = actionToDo, ActionWhenException = exception });
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Stops one thread. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public void StopOneThread()
        {
            PooledThreads = pooledThreads - 1;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Stops the threads. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public void StopThreads()
        {
            SetPooledThreads(0);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Creates new thread. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <returns> The new new thread. </returns>
        ///--------------------------------------------------------------------------------------------------
        private QueuedThread CreateNewThread()
        {
            QueuedThread newThread = new QueuedThread();
            newThread.OnSelectedEvent += ThreadFinishedWork;
            return newThread;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the next thread. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <returns> The next thread. </returns>
        ///--------------------------------------------------------------------------------------------------
        private QueuedThread GetNextThread()
        {
            lock (lockForFinish)
            {
                /* Give the next waiting Thread */
                if (waitingThreads.Count > 0)
                {
                    QueuedThread waitingThread = waitingThreads.Dequeue() as QueuedThread;
                    workingThreads.AddFirst(waitingThread);

                    return waitingThread;
                }

                /* If no waiting thread available, give the last working thread */
                if (workingThreads.Count > 0)
                {
                    QueuedThread workingThread = workingThreads.Last.Value;
                    workingThreads.Remove(workingThread);
                    workingThreads.AddFirst(workingThread);

                    return workingThread;
                }

                /* If nothing available return a new one */
                return CreateNewThread();
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Thread finished work. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="thread"> The thread. </param>
        ///--------------------------------------------------------------------------------------------------
        private void ThreadFinishedWork(QueuedThread thread)
        {
            lock (lockForFinish)
            {
                /* If there are more working threads than the current value of pooledThreads, stop it and don't add it to waitingThreads */
                if (workingThreads.Count + waitingThreads.Count > pooledThreads)
                {
                    if (workingThreads.Remove(thread))
                        thread.Stop();
                }
                else if (workingThreads.Remove(thread))
                    waitingThreads.Enqueue(thread);
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Sets pooled threads. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="value"> The value. </param>
        ///--------------------------------------------------------------------------------------------------
        private void SetPooledThreads(int value)
        {
            lock (lockForFinish)
            {
                /* If the new value is greater, add a new thread to the pool */
                if (value > pooledThreads)
                    waitingThreads.Enqueue(CreateNewThread());
                /* If is less, and there's a waiting thread, remove it, if not, the ThreadFinishedWork will fish the next working thread */
                else if (value < pooledThreads && waitingThreads.Count > 0)
                {
                    int threadsToStop = pooledThreads - value;
                    for (int i = 0; i < threadsToStop; i++)
                        waitingThreads.Dequeue().Stop();
                }

                pooledThreads = value;
            }
        }
    }
}
