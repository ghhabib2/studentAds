using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Classified.Web.Startup))]
namespace Classified.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
