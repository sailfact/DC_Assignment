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
    }

    [ServiceContract]
    public interface IDGPortalControllerCallback
    {

    }
}
