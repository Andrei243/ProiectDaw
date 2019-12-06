using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data.Entity.ModelConfiguration;

namespace DataAccess.Configurations
{
    internal static class PostConfiguration 
    {
        public static void Configure(EntityTypeConfiguration<Post> builder)
        {
                builder.HasIndex(e => e.AddingMoment)
                    .HasName("Post_AddingMoment");

                builder.Property(e => e.AddingMoment).HasColumnType("datetime");

                builder.Property(e => e.Confidentiality)
                    .HasMaxLength(20)
                    .IsUnicode(false);

            builder.HasRequired(d => d.User)
                .WithMany(p => p.Post)
                .HasForeignKey(d => d.UserId);
                    //.HasConstraintName("POST_USER_FK");
        }
    }
}
