using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DistributedGamePortal;

namespace DistributedGameServer
{
    [ServiceContract(CallbackContract = typeof(IDGServerControllerCallback))]
    public interface IDGServerController
    {
        [OperationContract]
        bool AddUser(User newUser, out string errMsg);
     }

    [ServiceContract]
    public interface IDGServerControllerCallback
    {

    }
}
