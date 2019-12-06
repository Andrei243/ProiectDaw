using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data.Entity.ModelConfiguration;

namespace DataAccess.Configurations
{
    internal static class CountyConfiguration
    {
        public static void Configure(EntityTypeConfiguration<County> builder)
        {
           
                builder.HasIndex(e => e.Name)
                    .HasName("County_Name");

                builder.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
           
        }
    }
}
