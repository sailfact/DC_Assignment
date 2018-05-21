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
        Guid Subscribe();

        [OperationContract(IsOneWay = true)]
        void Unsubscribe(Guid id);

        [OperationContract]
        void SelectHero(Guid id, Hero hero);

        [OperationContract]
        void AddUser(User newUser);

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
        void NotifyPlayerDied();

        [OperationContract(IsOneWay = true)]
        void NotifyGameEnded();

        [OperationContract(IsOneWay = true)]
        void ServerFull();

        [OperationContract]
        void TakeTurn(Hero hero, out int abilityIdx, out int targetIdx);

        [OperationContract(IsOneWay = true)]
        void NotifyGameStats(Boss boss, Dictionary<User, Hero> heros);
    }
}
