using Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Configurations
{
    internal static class GroupConfiguration
    {
        public static void Configure(EntityTypeConfiguration<Group> builder)
        {
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
        }

    }
}
