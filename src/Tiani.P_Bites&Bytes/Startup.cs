using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tiani.P_Bites_Bytes.Startup))]
namespace Tiani.P_Bites_Bytes
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
