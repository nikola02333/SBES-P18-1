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
        private static Dictionary<object, object> D;
        private static ServiceHost loadBalancer = null;
        private static ServiceHost wokerService = null;
        static IWorkerService workerChannel;
        public static void  Start()
        {
            // http://localhost:???/LoadBalancerService
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
            

         

            Console.ReadKey();

            Stop();
        }
    }
}
