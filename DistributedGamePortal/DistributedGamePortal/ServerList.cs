using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DistributedGamePortal
{
    [DataContract]
    public class Server
    {
        [DataMember]
        public int ServerID { get; set; }

        [DataMember]
        public string Url { get; set; }

        [DataMember]
        public string Name { get; set; }

        public Server(int id, string url, string name)
        {
            this.ServerID = id;
            this.Url = url;
            this.Name = name;
        }
    }

    [DataContract]
    public class ServerList
    {
        [DataMember]
        public List<Server> Servers { get; set; }

        [DataMember]
        public int ServerCount { get; set; }

        public ServerList()
        {
            Servers = new List<Server>();
            ServerCount = -1;
        }

        public void AddServer(Server server)
        {
            Servers.Add(server);
            ++ServerCount;
        }

        public Server GetServer(int index)
        {
            return Servers[index];
        }
    }
}
