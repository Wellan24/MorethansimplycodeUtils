using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Cartif.Threading
{
    public class AsyncJobsListener
    {
        private static Object locker = new Object();
        private int count;
        private Action allJobsFinished;

        public AsyncJobsListener(int jobsToListen, Action allJobsFinished)
        {
            count = jobsToListen;
            this.allJobsFinished = allJobsFinished;
        }

        public void FinishJob()
        {
            int c = Interlocked.Decrement(ref count);
            if (c == 0)
                allJobsFinished();
        }
    }
}
