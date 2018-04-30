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
    }

    [ServiceContract]
    public interface IDGPortalControllerCallback
    {

    }
}
