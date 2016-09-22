using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CIS219_LAB2.Startup))]
namespace CIS219_LAB2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
