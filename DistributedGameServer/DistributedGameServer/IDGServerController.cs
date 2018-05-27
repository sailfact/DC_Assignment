using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DistributedGamePortal;

namespace DistributedGameServer
{
    /// <summary>
    /// Server interaface
    /// </summary>
    [ServiceContract(CallbackContract = typeof(IDGServerControllerCallback))]
    public interface IDGServerController
    {
        [OperationContract]
        [FaultContract(typeof(GameServerFault))]
        bool Subscribe(User user);

        [OperationContract(IsOneWay = true)]
        void Unsubscribe(User user);

        [OperationContract]
        [FaultContract(typeof(GameServerFault))]
        void SelectHero(User user, Hero hero);
        
        [OperationContract]
        [FaultContract(typeof(GameServerFault))]
        List<Hero> GetHeroList();
    }

    /// <summary>
    /// Callback contract for the clients
    /// </summary>
    [ServiceContract]
    public interface IDGServerControllerCallback
    {
        [OperationContract(IsOneWay = true)]
        void NotifyClient(string msg);
        
        [OperationContract]
        bool NotifyGameEnded();

        [OperationContract]
        void TakeTurn(out Ability ability, out int targetIdx);

        [OperationContract(IsOneWay = true)]
        void NotifyGameStats(Boss boss, Dictionary<int, Hero> players, string lastAttacked);
    }
}
