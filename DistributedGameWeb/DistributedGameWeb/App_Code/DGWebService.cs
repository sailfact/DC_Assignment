using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DistributedGamePortal;
using DistributedGameServer;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DGWebService" in code, svc and config file together.
public class DGWebService : IDGWebService
{
    private IDGPortalController m_portal;
    private IDGServerController m_server;
    private Hero m_hero;
    private User m_user;

    public DGWebService()
    {
        m_server = null;
        m_hero = null;
        m_user = null;
        ConnectToPortal();
        Login();
    }

    private void Login()
    {
        throw new NotImplementedException();
    }

    public void ConnectToPortal()
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
            m_server.Subscribe(m_user);
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

    public bool VerifyUser(string username, string password, out User user)
    {
        return m_portal.VerifyUser(username, password, out user);
    }

    public void SelectServer()
    {
        throw new NotImplementedException();
    }

    public void SelectHero(Hero hero)
    {
        throw new NotImplementedException();
    }
}
