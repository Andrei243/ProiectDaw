using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data.Entity.ModelConfiguration;

namespace DataAccess.Configurations
{
    internal static class ReactionConfiguration
    {
        public static void Configure(EntityTypeConfiguration<Reaction> builder)
        {
            builder.HasKey(e => new { e.PostId, e.UserId });
            //.HasName("REACTION_PK");

            builder.HasRequired(d => d.Post)
                .WithMany(p => p.Reaction)
                .HasForeignKey(d => d.PostId);
            //.HasConstraintName("REACTION_POSTARE_FK");

            builder.HasRequired(d => d.User)
                .WithMany(p => p.Reaction)
                .HasForeignKey(d => d.UserId)
                .WillCascadeOnDelete();
                    //.HasConstraintName("REACTION_USER_FK");
        }
    }
}
