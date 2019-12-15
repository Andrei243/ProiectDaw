using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Text;

namespace DataAccess.Configurations
{
    internal static class IdentityUserLoginConfiguration
    {
        public static void Configure(EntityTypeConfiguration<IdentityUserLogin> builder)
        {
            builder.HasKey(e => new
            {
                e.UserId,
                e.ProviderKey
            });
        }

    }
}
