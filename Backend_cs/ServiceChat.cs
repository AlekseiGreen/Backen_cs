using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace Backend_cs
{
    public class ServiceChat : IServiceChat
    {
        public int connect()
        {
            throw new NotImplementedException();
        }

        public int Disconnect(int id)
        {
            throw new NotImplementedException();
        }

        public int SendMsg(string msg)
        {
            throw new NotImplementedException();
        }
    }
}
