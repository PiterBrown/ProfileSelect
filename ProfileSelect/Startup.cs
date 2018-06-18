using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProfileSelect.Startup))]
namespace ProfileSelect
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
