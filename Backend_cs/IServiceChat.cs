using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace Backend_cs // WCF
{

    [ServiceContract(CallbackContract = typeof(IServerChatCallback))]
    public interface IServiceChat
    {
        [OperationContract]
        int connect();

        [OperationContract]
        int Disconnect(int id);

        [OperationContract(IsOneWay = true)]
        int SendMsg(string msg);
    }

    public interface IServerChatCallback
    {
        [OperationContract]
        void MsgCallback(string msg);
    }

}

