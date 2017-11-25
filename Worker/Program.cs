using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace Worker
{
    class Program
    {
        static void Main(string[] args)
        {
          
            NetTcpBinding myBinding = new NetTcpBinding();
            EndpointAddress myEndpoint = new EndpointAddress("net.tcp://localhost:29000/IWorkerLB");
            ChannelFactory<IWorkerLB> myChannelFactory = new ChannelFactory<IWorkerLB>(myBinding, myEndpoint);
            IWorkerLB workerChannel = myChannelFactory.CreateChannel();
            
            Random rnd = new Random();
            int rdnm = rnd.Next(1,30);
           
           string myIp = "127.0.0." +rdnm+""; // args[0] 
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
