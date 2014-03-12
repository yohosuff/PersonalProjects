using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCTutorial.Startup))]
namespace MVCTutorial
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
