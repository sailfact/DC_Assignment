using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DistributedGamePortal
{
    [ServiceContract(CallbackContract = typeof(IDGPortalControllerCallback))]
    public interface IDGPortalController
    {
        [OperationContract]
        bool VerifyUser(string username, string password);

        [OperationContract]
        void VerifyUserAsync(string username, string passwd);

        [OperationContract]
        void VerifyUser_OnComplete(IAsyncResult res);

        [OperationContract]
        int GetServerID();
    }

    [ServiceContract]
    public interface IDGPortalControllerCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnVerifyUsersComplete(bool result);
    }
}
