using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cuillere.Startup))]
namespace Cuillere
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
