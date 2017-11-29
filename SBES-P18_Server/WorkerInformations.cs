using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;


namespace SBES_P18_Server
{
    [DataContract]
    public class WorkerInformations
    {

        private long free;
        public bool Free
        {
            get
            {
                Interlocked.Read(ref free);
                return free == 1L;
            }
            set
            {
                Interlocked.Exchange(ref free, value ? 1 : 0);  //uslovna dodela
            }
        }
        /// <summary>
        /// Provides (non-thread-safe) access to the backing value
        /// </summary>
        [DataMember]
        public int Id { get;  set; }
        [DataMember]
        public string Ip { get;  set; }
        [DataMember]
        public string URL { get;  set; }
    }
}
