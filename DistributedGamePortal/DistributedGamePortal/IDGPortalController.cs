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
        List<Server> GetServerList();

        [OperationContract]
        Server Subscribe();

        [OperationContract(IsOneWay = true)]
        void Unsubscribe(Server server);

        [OperationContract]
        void LogOff(User user);

        [OperationContract]
        FriendList GetFriendList(User user);
    }
}
