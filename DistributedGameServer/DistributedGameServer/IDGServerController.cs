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
    /// 
    /// </summary>
    [ServiceContract(CallbackContract = typeof(IDGServerControllerCallback))]
    public interface IDGServerController
    {
        [OperationContract]
        bool Subscribe(User user);

        [OperationContract(IsOneWay = true)]
        void Unsubscribe(User user);

        [OperationContract]
        void SelectHero(User user, Hero hero);
        
        [OperationContract]
        List<Hero> GetHeroList();
    }

    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]
    public interface IDGServerControllerCallback
    {
        [OperationContract(IsOneWay = true)]
        void NotifyPlayerDied(User player);

        [OperationContract]
        bool NotifyGameEnded();

        [OperationContract(IsOneWay = true)]
        void NotifyGameStart();

        [OperationContract(IsOneWay = true)]
        void ServerFull();

        [OperationContract]
        void TakeTurn(Hero hero, out int abilityIdx, out int targetIdx);

        [OperationContract(IsOneWay = true)]
        void NotifyGameStats(Boss boss, Dictionary<User, Hero> heros);
    }
}
