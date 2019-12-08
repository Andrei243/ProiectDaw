using Domain;
using System.Data.Entity.ModelConfiguration;

namespace DataAccess.Configurations
{
    internal static class CommentConfiguration 
    {
        public static void Configure(EntityTypeConfiguration<Comment> builder)
        {
            
                //builder.HasIndex(e => new { e.PostId, e.AddingMoment })
                  //  .HasName("Comment_AddingMoment");

                builder.Property(e => e.AddingMoment).HasColumnType("datetime");

                builder.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(3000);

            builder.HasRequired(d => d.Post)
                .WithMany(p => p.Comment)
                .HasForeignKey(d => d.PostId);
            //.HasConstraintName("COMMENT_POST_FK");

            builder.HasRequired(d => d.User)
                .WithMany(p => p.Comment)
                .HasForeignKey(d => d.UserId)
                //.HasConstraintName("COMMENT_USER_FK")
                .WillCascadeOnDelete();
          
        }
    }
}
