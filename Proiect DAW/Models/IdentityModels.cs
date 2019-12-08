using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Proiect_DAW.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : Domain.Users
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, Name));
            claims.Add(new Claim(ClaimTypes.Email, Email));
            claims.Add(new Claim(ClaimTypes.Role, Role.Name));
            
            userIdentity.AddClaims(claims);
            return userIdentity;
        }
    }

}