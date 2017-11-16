using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Common;

namespace SBES_P18_Server
{
    class Program
    {
        private static ServiceHost loadBalancer = null;
        public static void  Start()
        {
            loadBalancer = new ServiceHost(typeof(LoadBalancerService));

            loadBalancer.Open();
            Console.WriteLine("Load Balancer server is running...");
        }
        public static void Stop()
        {
            loadBalancer.Close();
        }
        static void Main(string[] args)
        {
            Start();
            Console.ReadKey();

            Stop();
        }
    }
}
