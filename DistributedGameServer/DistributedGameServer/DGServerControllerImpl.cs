using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DistributedGameServer
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     UseSynchronizationContext = false)]
    class DGServerControllerImpl : IDGServerController
    {
        public DGServerControllerImpl()
        {

        }
    }
}
