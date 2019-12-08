using DataAccess;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using Proiect_DAW.Models;

[assembly: OwinStartupAttribute(typeof(Proiect_DAW.Startup))]
namespace Proiect_DAW
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        
    }

}
