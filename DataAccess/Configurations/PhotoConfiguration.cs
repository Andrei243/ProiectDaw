using Domain;
using System.Data.Entity.ModelConfiguration;

namespace DataAccess.Configurations
{
    internal static class PhotoConfiguration
    {
        public static void Configure(EntityTypeConfiguration<Photo> builder)
        {
                builder.Property(e => e.Binary)
                    .IsRequired()
                    .HasColumnType("image");

            builder.HasOptional(d => d.Album)
                .WithMany(p => p.Photo)
                .HasForeignKey(d => d.AlbumId);
                    //.HasConstraintName("PHOTO_ALBUM_FK");

            builder.HasOptional(d => d.Post)
                .WithOptionalDependent(e=>e.Photo)
                //.WithOne(p => p.Photo)
                ;

                builder.Property(e => e.MIMEType)
                    .IsRequired()
                    .HasMaxLength(100);

        }
    }
}
