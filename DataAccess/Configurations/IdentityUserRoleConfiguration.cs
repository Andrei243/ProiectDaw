using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Text;

namespace DataAccess.Configurations
{
    internal static class IdentityUserRoleConfiguration
    {
        public static void Configure(EntityTypeConfiguration<IdentityUserRole> builder)
        {
            builder.HasKey(e => new
            {
                e.UserId,
                e.RoleId
            });
        }

    }
}
