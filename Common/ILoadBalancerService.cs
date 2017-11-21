using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
namespace Common
{
    [ServiceContract]
   public interface ILoadBalancerService
    {
        [OperationContract]
        bool Login(User u);
        [OperationContract]
        bool AddEntyty(Brojilo counter);
        [OperationContract]
        bool RemoveEntyty(Brojilo counter);
        [OperationContract]
        bool ChangeEntyty(Brojilo counterNew, Brojilo counterOld);

        [OperationContract]
        bool ChangeValueBrojila(string id, string potrosnja);

        [OperationContract]
        bool ChangeIdBrojila(string newId, string oldId);



    }
}
