using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DistributedGameData
{
    [ServiceContract]
    public interface IDGDataController
    {
        [OperationContract]
        bool GetUsernamePassword(int id, out string username, out string passwd, out string errMsg);

        [OperationContract]
        List<string> GetFriendsByID(int id, out string errMsg);

        [OperationContract]
        int GetNumHeroes(out string errMsg);

        [OperationContract]
        int GetNumBosses(out string errMsg);

        [OperationContract]
        int GetNumUsers(out string errMsg);

        [OperationContract]
        string GetHeroNameByID(int id, out string errMsg);

        [OperationContract]
        string GetBossNameByID(int id, out string errMsg);

        [OperationContract]
        bool GetHeroStatsByID(int id, out int def, out int hp, out int moveNum, out string errMsg);

        [OperationContract]
        bool GetBossStatsByID(int id, out int def, out int hp, out int damage, out char targetPref, out string errMsg);

        [OperationContract]
        bool GetMovesByIDAndIndex(int id, int index, out int value, out string description, out char type, out char target, out string errMsg);
    }
}
