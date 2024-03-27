using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    internal class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasOne(p => p.Owner);


            builder.Property(p => p.Name)
                .HasMaxLength(60)
                .IsRequired(true);

            builder.Property(p => p.Name)
                .HasMaxLength(60)
                .IsRequired(true);

            builder.Property(p => p.Description)
                .HasMaxLength(200)
                .IsRequired(false);
        }
    }
}
