using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DistributedGameWeb.Startup))]
namespace DistributedGameWeb
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
