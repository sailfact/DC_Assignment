using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DistributedGameServer
{
    [ServiceContract(CallbackContract = typeof(IDGServerControllerCallback))]
    public interface IDGServerController
    {
    }

    [ServiceContract]
    public interface IDGServerControllerCallback
    {

    }
}
