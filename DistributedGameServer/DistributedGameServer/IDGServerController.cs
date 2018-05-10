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
        void SelectHero(Hero hero);

        [OperationContract]
        void GetGameStats(out Boss boss, out Dictionary<User, Hero> heros);
    }

    [ServiceContract]
    public interface IDGServerControllerCallback
    {
        [OperationContract(IsOneWay = true)]
        void NotifyPlayerDied();

        [OperationContract(IsOneWay = true)]
        void NotifyGameEnded();
    }
}
