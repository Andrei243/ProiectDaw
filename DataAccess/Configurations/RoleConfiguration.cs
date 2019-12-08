using Domain;
using System.Data.Entity.ModelConfiguration;

namespace DataAccess.Configurations
{
    internal static class RoleConfiguration
    {
        public static void Configure(EntityTypeConfiguration<Role> builder)
        {
                builder.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
        }
    }
}
