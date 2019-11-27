using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Common;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Domain
{
    public partial class Users : IdentityUser,IEntity
    {
        public Users()
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
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Password { get; set; }
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
    }
}
