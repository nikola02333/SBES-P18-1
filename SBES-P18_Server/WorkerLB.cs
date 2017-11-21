using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WcfService3;

namespace SBES_P18_Server
{
    public class WorkerLB : IWorkerLB

    {
        private int workerCounter = 0;
       public static ConcurrentDictionary<string, WorkerInformations> workers = new ConcurrentDictionary<string, WorkerInformations>();

        public string Register(string ip)
        {
           
        

                int newVal = Interlocked.Increment(ref workerCounter);


                WorkerInformations info = new WorkerInformations();
                string url = "net.tcp://" + ip + ":27000/IWorkerService" + newVal;

                info.Id = workerCounter;
                info.Ip = ip;
                info.Free = true;
                info.URL = url;


                workers.TryAdd(url, info);

                Console.WriteLine("Worker sa id-om: " + info.Id + "je spreman za rad,poyy");


                return url;

        }

        public void UnRegister(string url)
        {
            WorkerInformations worker;
            workers.TryRemove(url, out worker);
            Console.WriteLine("Worker sa url-om {0} se odjavio", url);

        }
    }
}
