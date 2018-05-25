using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DistributedGamePortal;
using DistributedGameServer;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDGPortalService" in both code and config file together.
[ServiceContract]
public interface IDGPortalService
{
    [OperationContract]
    bool VerifyUser(string username, string password, out User user);

    [OperationContract]
    List<Server> GetServerList();
    
    [OperationContract]
    void LogOff(User user);

    [OperationContract]
    FriendList GetFriendList(User user);
}
