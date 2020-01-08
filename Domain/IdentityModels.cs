using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Domain
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser, IEntity
    {
        public ApplicationUser()
        {
            Album = new HashSet<Album>();
            Comment = new HashSet<Comment>();
            FriendshipIdReceiverNavigation = new HashSet<Friendship>();
            FriendshipIdSenderNavigation = new HashSet<Friendship>();
            InterestsUsers = new HashSet<InterestsUsers>();
            Post = new HashSet<Post>();
            Reaction = new HashSet<Reaction>();
        }
        [Required]
        public override string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public int RoleId { get; set; }
        [Required]
        public DateTime BirthDay { get; set; }
        public int? LocalityId { get; set; }
        [Required]
        public string SexualIdentity { get; set; }
        [Required]
        [Column("Vizibility")]
        public string Confidentiality { get; set; }
        public int? PhotoId { get; set; }
        public bool IsBanned { get; set; }
        public virtual Photo Photo { get; set; }
        public virtual Locality Locality { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Album> Album { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<Friendship> FriendshipIdReceiverNavigation { get; set; }
        public virtual ICollection<Friendship> FriendshipIdSenderNavigation { get; set; }
        public virtual ICollection<InterestsUsers> InterestsUsers { get; set; }
        public virtual ICollection<Post> Post { get; set; }
        public virtual ICollection<Reaction> Reaction { get; set; }
        public virtual ICollection<Message> SentMessages { get; set; }
        public virtual ICollection<Message> ReceivedMessages { get; set; }

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