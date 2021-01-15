using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Mayank_Task.Startup))]
namespace Mayank_Task
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
