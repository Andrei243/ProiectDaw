using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data.Entity.ModelConfiguration;

namespace DataAccess.Configurations
{
    internal static class FriendshipConfiguration
    {
        public static void Configure(EntityTypeConfiguration<Friendship> builder)
        {
            builder.HasKey(e => new { e.IdSender, e.IdReceiver });
            //.HasName("FRIENDSHIP_PK");

            builder.HasRequired(d => d.IdReceiverNavigation)
                .WithMany(p => p.FriendshipIdReceiverNavigation)
                .HasForeignKey(d => d.IdReceiver)
                .WillCascadeOnDelete();
            //.HasConstraintName("FRIENDSHIP_USER_RECEIVER_FK");

            builder.HasRequired(d => d.IdSenderNavigation)
                .WithMany(p => p.FriendshipIdSenderNavigation)
                .HasForeignKey(d => d.IdSender)
                .WillCascadeOnDelete();
                   // .HasConstraintName("FRIENDSHIP_USER_SENDER_FK");
        }
    }
}
