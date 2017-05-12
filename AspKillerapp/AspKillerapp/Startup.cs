using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AspKillerapp.Startup))]
namespace AspKillerapp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
