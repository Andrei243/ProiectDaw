using Domain;
using System.Data.Entity.ModelConfiguration;

namespace DataAccess.Configurations
{
    internal static class UserConfiguration
    {
        public static void Configure(EntityTypeConfiguration<User> builder)
        {
                //builder.HasIndex(e => e.Email)
                  //  .HasName("User_Email");

                //builder.HasIndex(e => new { e.Name, e.Surname })
                  //  .HasName("User_Search");

                builder.Property(e => e.BirthDay).HasColumnType("date");

                builder.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                builder.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                //builder.Property(e => e.Password)
                //    .IsRequired()
                //    .HasMaxLength(100)
                //    .IsUnicode(false);

                builder.Property(e => e.SexualIdentity)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                builder.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                builder.Property(e => e.Confidentiality)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

            builder.HasOptional(e => e.Photo)
                .WithOptionalPrincipal(u => u.User);
                    //.HasForeignKey<Users>(e => e.PhotoId)
                    //.IsRequired(false)
                    //.HasConstraintName("USER_PROFILE_PHOTO_FK");

            builder.HasRequired(d => d.Locality)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.LocalityId);
            //.HasConstraintName("USER_LOCALITY_FK");

            builder.HasRequired(d => d.Role)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId);
                    //.HasConstraintName("USER_ROLE_FK");
        }
    }
}
