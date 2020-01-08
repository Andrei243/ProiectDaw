using Domain;
using System.Data.Entity.ModelConfiguration;

namespace DataAccess.Configurations
{
    internal static class MessageConfiguration
    {
        public static void Configure(EntityTypeConfiguration<Message> builder)
        {
            builder.Property(e => e.SendingMoment)
                .HasColumnType("datetime");

            builder.Property(e => e.Content)
                .IsRequired();

            builder.HasRequired(d => d.Sender)
                .WithMany(p => p.SentMessages)
                .HasForeignKey(d => d.SentByUserId);

            builder.HasRequired(d => d.Receiver)
                .WithMany(p => p.ReceivedMessages)
                .HasForeignKey(d => d.Receiver);
        }
    }
}
