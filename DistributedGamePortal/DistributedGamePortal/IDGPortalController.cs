using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DistributedGamePortal
{
    [ServiceContract(CallbackContract=typeof(IDGPortalControllerCallback))]
    public interface IDGPortalController
    {
        [OperationContract]
        bool VerifyUser(string username, string password, out User user);

        [OperationContract]
        void VerifyUserAsync(string username, string password);
        
        [OperationContract]
        Server GetServerID();

        [OperationContract]
        FriendList GetFriendList();

        [OperationContract]
        ServerList GetServerList();
    }

    [ServiceContract]
    public interface IDGPortalControllerCallback
    {
        [OperationContract(IsOneWay = true)]
        void VerifyUserOnComplete(bool result, User user);
    }
}
