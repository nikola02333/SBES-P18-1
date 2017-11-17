using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Common
{
    [ServiceContract]
<<<<<<< HEAD
    public interface IWorkerRoleService
    {
        [OperationContract]
        double Price(int p, List<Tarife> lt);
        [OperationContract]
        int Work(int id);
        [OperationContract]
        void StartId(int id);
        

    } 
}

=======
    public   interface IWorkerRoleService
    {
        [OperationContract]
        int Work();

    }
}
>>>>>>> b29379756fc0829bfe43b745aac3d329b6b2f139
