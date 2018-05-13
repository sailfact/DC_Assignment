using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DistributedGamePortal
{
    [ServiceContract]
    public interface IDGPortalController
    {
        [OperationContract]
        bool VerifyUser(string username, string password, out User user);

        [OperationContract]
        Server GetServerInfo();
        
        [OperationContract]
        ServerList GetServerList();

        [OperationContract]
        void AddServerInfo(Server server);
    }
}
