using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GreenLight.Startup))]
namespace GreenLight
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
