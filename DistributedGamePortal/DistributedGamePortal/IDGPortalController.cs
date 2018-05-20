using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Net.Security;

namespace DistributedGamePortal
{
    [ServiceContract]
    public interface IDGPortalController
    {
        [OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign)]
        bool VerifyUser(string username, string password, out User user);

        [OperationContract]
        Server GetServerInfo();
        
        [OperationContract]
        ServerList GetServerList();

        [OperationContract]
        void AddServerInfo(Server server);
    }
}
