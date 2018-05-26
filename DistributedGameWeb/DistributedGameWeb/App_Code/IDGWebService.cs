using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DistributedGamePortal;
using DistributedGameServer;

[ServiceContract]
public interface IDGWebService
{
    [OperationContract]
    bool VerifyUser(string username, string password, out User user);

    [OperationContract]
    FriendList GetFriendList(User user);

    [OperationContract]
    void SelectServer();

    [OperationContract]
    void SelectHero(Hero hero);
}
