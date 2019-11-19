using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LostObjects.Backend.Startup))]
namespace LostObjects.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
