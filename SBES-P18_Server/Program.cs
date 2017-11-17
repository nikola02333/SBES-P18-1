using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Common;

<<<<<<< HEAD

=======
>>>>>>> b29379756fc0829bfe43b745aac3d329b6b2f139
namespace SBES_P18_Server
{
    class Program
    {
<<<<<<< HEAD
        private static Dictionary<object, object> D;
=======
>>>>>>> b29379756fc0829bfe43b745aac3d329b6b2f139
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
<<<<<<< HEAD

            Start();
            var CLB = new Queue<int>(100);  //Queue sa ID-evima korisnika koji zele obradu podataka

            Dictionary<int, int> LBW = new Dictionary<int, int>(100);//Dictionary sa ID i potrosnjom  koji se salje workerima




=======
            Start();
>>>>>>> b29379756fc0829bfe43b745aac3d329b6b2f139
            Console.ReadKey();

            Stop();
        }
    }
}
