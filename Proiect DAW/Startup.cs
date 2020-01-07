using DataAccess;
using Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using Proiect_DAW.Models;
using System;
using System.IO;

[assembly: OwinStartup(typeof(Proiect_DAW.Startup))]
namespace Proiect_DAW
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            string root = AppDomain.CurrentDomain.BaseDirectory;
            var physicalFileSystem = new PhysicalFileSystem(Path.Combine(root, "wwwroot"));
            var options = new FileServerOptions
            {
                RequestPath = PathString.Empty,
                EnableDefaultFiles = true,
                FileSystem = physicalFileSystem

            };
            options.StaticFileOptions.FileSystem = physicalFileSystem;
            options.StaticFileOptions.ServeUnknownFileTypes = false;
            app.UseFileServer(options);
            createAdminUserAndApplicationRoles();
        }

        private void createAdminUserAndApplicationRoles()
        {
            SocializRContext context = new SocializRContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            // Se adauga rolurile aplicatiei
            if (!roleManager.RoleExists("admin"))
            {
                // Se adauga rolul de administrator
                var role = new IdentityRole();
                role.Name = "admin";
                roleManager.Create(role);
                // se adauga utilizatorul administrator
                var user = new ApplicationUser();
                user.Name = "admin";
                user.Surname = "admin admin";
                user.UserName = "admin";
                user.Email = "admin@admin.com";
                user.BirthDay = new DateTime(1111, 11, 11);
                user.SexualIdentity = "Nespecificat";
                user.Confidentiality = "public";
                user.RoleId = 1;
                user.LocalityId = 1;
                
                var adminCreated = UserManager.Create(user, "administrator");
                if (adminCreated.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "admin");
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
