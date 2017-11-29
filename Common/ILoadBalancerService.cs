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
        bool AddEntyty(Brojilo counter);
        [OperationContract]
        bool ChangeEntyty(Brojilo counterNew, Brojilo counterOld);
        [OperationContract]
        double Process_Id(int id_Brojila);
		[OperationContract]
        EnumType Detekcija();
        [OperationContract]
        Brojilo SearchId(int id);
        [OperationContract]
        bool RemoveEntyty(int id);
    }
}
