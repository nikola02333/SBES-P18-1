using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace SBES_P18_Server
{
    public class WorkerInformations
    {

        private int free;

        /// <summary>
        /// Provides (non-thread-safe) access to the backing value
        /// </summary>
        public bool Free
        {
            get
            {
                long result = 0;

                Interlocked.Read(ref result);
                return result == 1;
            }
            set
            {
                Interlocked.Exchange(ref free, value ? 1 : 0);  //uslovna dodela
            }
        }


        public int Id { get;  set; }
        public string Ip { get;  set; }
        public string URL { get;  set; }
    }
}
