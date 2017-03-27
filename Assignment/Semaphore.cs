using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assignment
{
    public class Semaphore
    {
        private int count = 1; // set semaphore to available

        public void Wait()
        {
            lock (this)
            {
                // wait if semaphore unavailable
                while (count == 0)
                    Monitor.Wait(this);
                count = 0;
            }
        }

        public void Signal()
        {
            lock (this)
            {
                count = 1;
                Monitor.Pulse(this);
            }
        }

        public void Start()
        {
        }

    }// end class Semaphore
}

