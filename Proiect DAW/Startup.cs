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
            createAdminUserAndApplicationRoles();
        }

        private void createAdminUserAndApplicationRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
 
            if (!roleManager.RoleExists("Admin"))
            {
               
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
                
                var user = new ApplicationUser();
                user.UserName = "admin@admin.com";
                user.Email = "admin@admin.com";
                var adminCreated = UserManager.Create(user, "Administrator1!");
                if (adminCreated.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Admin");
                }
            }
            if (!roleManager.RoleExists("public"))
            {
                var role = new IdentityRole();
                role.Name = "public";
                roleManager.Create(role);
            }
        }
    }

}
