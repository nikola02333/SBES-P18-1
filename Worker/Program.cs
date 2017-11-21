using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WcfService3;

namespace Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            WorkerService obj = new WorkerService();

            // napraviti Kanal ka LoadBalanceru
            // pozvati metodu register
            // kada metoda vrati URL
            // napraviti ServiceHost sa tim URLom
           // BasicHttpBinding myBinding = new BasicHttpBinding();
            NetTcpBinding myBinding = new NetTcpBinding();

            EndpointAddress myEndpoint = new EndpointAddress("net.tcp://localhost:29000/IWorkerLB");

            ChannelFactory<IWorkerLB> myChannelFactory = new ChannelFactory<IWorkerLB>(myBinding, myEndpoint);

            IWorkerLB workerChannel = myChannelFactory.CreateChannel();

            //////////////////////////////////////////////

            string myIp = "127.0.0.1"; // args[0]  
            string workerServiceURL = workerChannel.Register(myIp);     //ovde puca


            Uri baseAddress = new Uri(workerServiceURL);
   

            using (ServiceHost serviceHost = new ServiceHost(typeof(WorkerService), baseAddress))
            {
                serviceHost.AddServiceEndpoint(typeof(IWorkerService), new NetTcpBinding(), workerServiceURL);
                serviceHost.Open();
                Console.WriteLine("Press <enter> to terminate service");
                Console.ReadLine();
                serviceHost.Close();

                workerChannel.UnRegister(workerServiceURL);
                Console.WriteLine("Worker sa url-om {0} :", workerServiceURL);
            }



        }
    }
}
