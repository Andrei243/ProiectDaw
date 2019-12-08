using Domain;
using System.Data.Entity.ModelConfiguration;

namespace DataAccess.Configurations
{
    internal static class InterestConfiguration
    {
        public static void Configure(EntityTypeConfiguration<Interest> builder)
        {
                //builder.HasIndex(e => e.Name)
                  //  .HasName("Interest_Denumire");

                builder.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
        }
    }
}
