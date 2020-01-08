using Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Configurations
{
    internal static class ApplicationUserGroupsConfiguration
    {
        public static void Configure(EntityTypeConfiguration<ApplicationUserGroup> builder)
        {
            builder.HasKey(e =>
            new
            {
                e.GroupId,
                e.UserId
            });

            builder.HasRequired(e => e.Group)
                .WithMany(e => e.Users)
                .HasForeignKey(e => e.GroupId);
            builder.HasRequired(e => e.User)
                .WithMany(e => e.Groups)
                .HasForeignKey(e => e.UserId);

        }
    }
}
