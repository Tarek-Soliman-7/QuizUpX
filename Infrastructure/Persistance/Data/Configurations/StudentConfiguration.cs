using Domain.Entities.IdentityModule;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Data.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.UniversityCode)
                   .IsRequired()
                   .HasMaxLength(50);
            builder.Property(s => s.Pin)
                   .IsRequired()
                   .HasMaxLength(10);


            builder.HasIndex(s => s.UniversityCode)
                   .IsUnique();

            builder.Property(s => s.IsActive)
                   .HasDefaultValue(true);

            builder.HasMany(s => s.Attempts)
                   .WithOne(a => a.Student)
                   .HasForeignKey(a => a.StudentId)
                   .OnDelete(DeleteBehavior.Restrict);




        }
    }

}
