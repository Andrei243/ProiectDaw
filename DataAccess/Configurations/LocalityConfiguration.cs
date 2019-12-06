using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data.Entity.ModelConfiguration;

namespace DataAccess.Configurations
{
    internal static class LocalityConfiguration 
    {
        public static void Configure(EntityTypeConfiguration<Locality> builder)
        {
                builder.HasIndex(e => e.Name)
                    .HasName("Localitate_Nume");

                builder.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

            builder.HasRequired(d => d.County)
                .WithMany(p => p.Locality)
                .HasForeignKey(d => d.CountyId);
                    //.HasConstraintName("LOCALITY_COUNTY_FK");
        }
    }
}
