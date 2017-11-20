using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Common;
using System.IdentityModel.Policy;
using System.ServiceModel.Description;
using SecurityManager;

namespace SBES_P18_Server
{
    class Program
    {
        private static Dictionary<object, object> D;
        private static ServiceHost loadBalancer = null;
        public static void  Start()
        {
            loadBalancer = new ServiceHost(typeof(LoadBalancerService));
            RolesConfig rc = new RolesConfig();
            ///
            loadBalancer.Authorization.ServiceAuthorizationManager = new CustomAuthorizationManager();

            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new CustomAuthorizationPolicy());
            loadBalancer.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();

            loadBalancer.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;

            loadBalancer.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            loadBalancer.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
            ///
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
