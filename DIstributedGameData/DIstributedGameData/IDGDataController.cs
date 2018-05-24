using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Net.Security;

namespace DistributedGameData
{
    /// <summary>
    /// IDGDataController
    /// Server interface for Distributed Game Data
    /// </summary>
    [ServiceContract]
    public interface IDGDataController
    {
        [OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign)]
        [FaultContract(typeof(DataServerFault))]
        void GetUsernamePassword(int id, out string username, out string passwd);

        [OperationContract]
        [FaultContract(typeof(DataServerFault))]
        List<string> GetFriendsByID(int id);

        [OperationContract]
        [FaultContract(typeof(DataServerFault))]
        int GetNumHeroes();

        [OperationContract]
        [FaultContract(typeof(DataServerFault))]
        int GetNumBosses();

        [OperationContract]
        [FaultContract(typeof(DataServerFault))]
        int GetNumUsers();

        [OperationContract]
        [FaultContract(typeof(DataServerFault))]
        string GetHeroNameByID(int id);

        [OperationContract]
        [FaultContract(typeof(DataServerFault))]
        string GetBossNameByID(int id);

        [OperationContract]
        [FaultContract(typeof(DataServerFault))]
        void GetHeroStatsByID(int id, out int def, out int hp, out int moveNum);

        [OperationContract]
        [FaultContract(typeof(DataServerFault))]
        void GetBossStatsByID(int id, out int def, out int hp, out int damage, out char targetPref);

        [OperationContract]
        [FaultContract(typeof(DataServerFault))]
        void GetMovesByIDAndIndex(int id, int index, out int value, out string description, out char type, out char target);
    }
}
