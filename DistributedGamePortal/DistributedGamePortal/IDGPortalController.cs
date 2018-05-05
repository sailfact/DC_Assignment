using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DistributedGamePortal
{
    public interface IDGPortalController
    {
        [OperationContract]
        bool VerifyUser(string username, string password);
        
        [OperationContract]
        int GetServerID();

        [OperationContract]
        FriendList GetFriendList()
        {
            
1       }
    }
}
