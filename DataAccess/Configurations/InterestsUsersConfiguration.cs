using Domain;
using System.Data.Entity.ModelConfiguration;

namespace DataAccess.Configurations
{
    internal static class InterestsUsersConfiguration
    {
        public static void Configure(EntityTypeConfiguration<InterestsUsers> builder)
        {
            builder.HasKey(e => new { e.InterestId, e.UserId });
            //.HasName("INTEREST_LINK_PK");

            builder.HasRequired(d => d.Interest)
                .WithMany(p => p.InterestsUsers)
                .HasForeignKey(d => d.InterestId);
            // .HasConstraintName("INTEREST_LINK_INTERES_FK");

            builder.HasRequired(d => d.User)
                .WithMany(p => p.InterestsUsers)
                .HasForeignKey(d => d.UserId);
                    //.HasConstraintName("INTEREST_LINK_User_FK");
        }
    }
}
