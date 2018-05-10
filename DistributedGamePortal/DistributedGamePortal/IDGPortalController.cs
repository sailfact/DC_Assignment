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
        Server GetServerInfo();
        
        [OperationContract]
        ServerList GetServerList();

        [OperationContract]
        void AddServerInfo(Server server);
    }

    [ServiceContract]
    public interface IDGPortalControllerCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnCompleteVerifyUsers(bool result, User user);
    }
}
