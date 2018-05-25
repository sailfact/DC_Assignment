using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DistributedGamePortal;
using DistributedGameServer;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DGPortalService" in code, svc and config file together.
public class DGPortalService : IDGPortalService
{
    private IDGPortalController m_portal;
    private IDGServerController m_server;
    private User m_user;
    private Hero m_hero;

    public DGPortalService()
    {
        ChannelFactory<IDGPortalController> channelFactory;
        NetTcpBinding tcpBinding = new NetTcpBinding();
        string url = "net.tcp://localhost:50002/DGPortal";
        try
        {
            channelFactory = new ChannelFactory<IDGPortalController>(tcpBinding, url);   // bind url to channel factory
            m_portal = channelFactory.CreateChannel();  // create portal on remote server
        }
        catch (ArgumentNullException)
        {
            
        }
        catch (InvalidOperationException)
        {
            
        }
        catch (EndpointNotFoundException)
        {
            
        }
    }

    public void ConnectToServer(Server server)
    {
        DuplexChannelFactory<IDGServerController> channelFactory;

        NetTcpBinding tcpBinding = new NetTcpBinding();
        string url = server.Url;

        try
        {
            channelFactory = new DuplexChannelFactory<IDGServerController>(new InstanceContext(this), tcpBinding, url);   // bind url to channel factory
            m_server = channelFactory.CreateChannel();  // create portal on remote server
        }
        catch (ArgumentNullException)
        {
        }
        catch (InvalidOperationException)
        {
        }
        catch (EndpointNotFoundException)
        {
        }
    }

    public FriendList GetFriendList(User user)
    {
        return m_portal.GetFriendList(user);
    }

    public List<Server> GetServerList()
    {
        return m_portal.GetServerList();
    }

    public void LogOff(User user)
    {
        m_portal.LogOff(user);
    }

    public bool VerifyUser(string username, string password, out User user)
    {
        return m_portal.VerifyUser(username, password, out user);
    }
    
    public void Login()
    {

    }

    public Server SelectServer()
    {
        return null;
    }
}
