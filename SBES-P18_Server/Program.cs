using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Common;
using System.Threading;
using SecurityManager;
using System.IdentityModel.Policy;
using System.ServiceModel.Description;

namespace SBES_P18_Server
{
    class Program
    {
        private static ServiceHost loadBalancer = null;
        private static ServiceHost wokerService = null;
        public static void  Start()
        {
            loadBalancer = new ServiceHost(typeof(LoadBalancerService));
            RolesConfig rc = new RolesConfig();
            loadBalancer.Authorization.ServiceAuthorizationManager = new CustomAuthorizationManager();

            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new CustomAuthorizationPolicy());
            loadBalancer.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();

            loadBalancer.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;

            loadBalancer.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            loadBalancer.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
            loadBalancer.Open();
            
            wokerService = new ServiceHost(typeof(WorkerLB));

            wokerService.Open();
            Console.WriteLine("Load Balancer server is running...");
        }
       
        public static void Stop()
        {
            loadBalancer.Close();
        }
        static void Main(string[] args)
        {
            Start();
            Console.WriteLine("Unesite (1) za EBC enkripciju/dekripciju ili (2) za CBC enkripciju/dekripciju");
             string opcija = Console.ReadLine();
            if(opcija =="1")
            {
                SymmetricAlgorithmsCBC.Program.StartCBC();
            }
            if (opcija == "2")
            {
                SymmetricAlgorithmsCBC.Program.StartCBC();
            }
            Console.ReadKey();
            Stop();
        }
    }
}
