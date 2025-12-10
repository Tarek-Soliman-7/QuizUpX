using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Data.Configurations
{
    public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.ToTable("Subjects");

            builder.HasKey(s => s.id);

            builder.Property(s => s.name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.description)
                .HasMaxLength(1000);

            builder.HasMany(s => s.Questions)
                   .WithOne(q => q.Subject)
                   .HasForeignKey(q => q.subjectId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
