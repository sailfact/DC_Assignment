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
        bool VerifyUser(string username, string password);
        
        [OperationContract]
        int GetServerID();

        [OperationContract]
        FriendList GetFriendList();
    }

    [ServiceContract]
    public interface IDGPortalControllerCallback
    {
        [OperationContract(IsOneWay = true)]
        void AddUser(User newUser);
    }
}
