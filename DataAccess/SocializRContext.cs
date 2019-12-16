using Domain;
using DataAccess.Configurations;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DataAccess
{
    public partial class SocializRContext : IdentityDbContext<ApplicationUser>
    {
        public SocializRContext()
            :base("SocializR")
        {
        }


        public virtual DbSet<Album> Album { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<County> County { get; set; }
        public virtual DbSet<ErrorLogCustom> ErrorLogCustom { get; set; }
        public virtual DbSet<Friendship> Friendship { get; set; }
        public virtual DbSet<Interest> Interest { get; set; }
        public virtual DbSet<InterestsUsers> InterestsUsers { get; set; }
        public virtual DbSet<Locality> Locality { get; set; }
        public virtual DbSet<Photo> Photo { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<Reaction> Reaction { get; set; }
        public virtual DbSet<Role> Role { get; set; }

        public static SocializRContext Create()
        {
            return new SocializRContext();
        }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            AlbumConfiguration.Configure(modelBuilder.Entity<Album>());
            CommentConfiguration.Configure(modelBuilder.Entity<Comment>());
            CountyConfiguration.Configure(modelBuilder.Entity<County>());
            ErrorLogCustomConfiguration.Configure(modelBuilder.Entity<ErrorLogCustom>());
            FriendshipConfiguration.Configure(modelBuilder.Entity<Friendship>());
            InterestConfiguration.Configure(modelBuilder.Entity<Interest>());
            InterestsUsersConfiguration.Configure(modelBuilder.Entity<InterestsUsers>());
            LocalityConfiguration.Configure(modelBuilder.Entity<Locality>());
            PhotoConfiguration.Configure(modelBuilder.Entity<Photo>());
            PostConfiguration.Configure(modelBuilder.Entity<Post>());
            ReactionConfiguration.Configure(modelBuilder.Entity<Reaction>());
            RoleConfiguration.Configure(modelBuilder.Entity<Role>());
            UserConfiguration.Configure(modelBuilder.Entity<ApplicationUser>());
            IdentityUserLoginConfiguration.Configure(modelBuilder.Entity<IdentityUserLogin>());
            IdentityUserRoleConfiguration.Configure(modelBuilder.Entity<IdentityUserRole>());
        }
    }
}
