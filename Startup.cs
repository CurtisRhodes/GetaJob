using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GetaJob.Startup))]
namespace GetaJob
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           // ConfigureAuth(app);
        }
    }
}
