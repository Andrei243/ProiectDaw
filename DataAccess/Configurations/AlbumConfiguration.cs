using Domain;
using System.Data.Entity.ModelConfiguration;

namespace DataAccess.Configurations
{
    internal static class AlbumConfiguration
    {
        public static void Configure(this EntityTypeConfiguration<Album> builder)
        {
          
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.HasRequired(d => d.User)
                .WithMany(p => p.Album)
                .HasForeignKey(d => d.UserId);
                //.HasConstraintName("ALBUM_USER_FK");
            


        }

       

    }
}
