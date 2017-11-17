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
        private static Dictionary<object, object> D;
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
            var CLB = new Queue<int>(100);  //Queue sa ID-evima korisnika koji zele obradu podataka

            Dictionary<int, int> LBW = new Dictionary<int, int>(100);//Dictionary sa ID i potrosnjom  koji se salje workerima




            Console.ReadKey();

            Stop();
        }
    }
}
