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
        void SelectHero(Hero hero, User user);

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
        void TakeTurn(User user, out int abilityIdx, out int targetIdx);

        [OperationContract(IsOneWay = true)]
        void NotigyGameStats(out Boss boss, out Dictionary<User, Hero> heros);
    }
}
