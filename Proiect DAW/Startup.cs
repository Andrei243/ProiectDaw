using DataAccess;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using Proiect_DAW.Models;
using System;

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
            SocializRContext context = new SocializRContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<Domain.User>(new UserStore<Domain.User>(context));
            // Se adauga rolurile aplicatiei
            if (!roleManager.RoleExists("admin"))
            {
                // Se adauga rolul de administrator
                var role = new IdentityRole();
                role.Name = "admin";
                roleManager.Create(role);
                // se adauga utilizatorul administrator
                var user = new Domain.User();
                user.Name = "admin";
                user.Surname = "admin admin";
                user.UserName = "admin";
                user.Email = "admin@admin.com";
                user.BirthDay = new DateTime(1111, 11, 11);
                user.SexualIdentity = "Nespecificat";
                user.Confidentiality = "public";
                user.LocalityId = 1;
                IdentityResult adminCreated = null;
                try
                {
                    adminCreated = UserManager.Create(user, "admin");
                }
                catch(Exception e)
                {
                    int x = 0;
                }
                if (adminCreated.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "admin");
                }
            }
            
            if (!roleManager.RoleExists("Public"))
            {
                var role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);
            }
        }
    }

}
