using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Common
{

    [ServiceContract] //nacini da server tj. LB okine metodu od nekog prethodnog poznatog workera,zato sto se workeri prvo reg.
    public interface IWorkerLB
    {

        [OperationContract(IsOneWay =false)]
        string Register(string ip);

        [OperationContract(IsOneWay = false)]
        void UnRegister(string url);

    }


    [ServiceContract]
    public interface IWorkerService
    {
        //metode koje lb poziva na workeru i on to odradi sta treba i vrati cenu samo

        [OperationContract(IsOneWay = false)]
        double GetPrice(string potrosnja);

    }



}
