using Domain;
using DataAccess.Configurations;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace DataAccess
{
    public partial class SocializRContext : IdentityDbContext<Users>
    {
        public SocializRContext()
            :base("Server=(LocalDB)\\MSSQLLocalDB;Database=SocializR;Trusted_Connection=true;Connection Timeout=3600")
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



//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
            
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB;Database=SocializR;Trusted_Connection=true;Connection Timeout=3600");

//            }
//        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");
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
            UserConfiguration.Configure(modelBuilder.Entity<Users>());

        }
    }
}
