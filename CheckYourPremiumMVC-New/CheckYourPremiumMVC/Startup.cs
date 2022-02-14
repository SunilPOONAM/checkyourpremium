using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CheckYourPremiumMVC.Startup))]
namespace CheckYourPremiumMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
