using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Enums;
using System.Reflection.Emit;

namespace Persistance.Data.Configurations
{
    public class AttemptConfiguration : IEntityTypeConfiguration<Attempt>
    {
        public void Configure(EntityTypeBuilder<Attempt> builder)
        {
            builder.ToTable("Attempts");
            builder.HasKey(e => e.Id);

            

            builder.Property(e => e.Status)
                   .HasMaxLength(50)
                   .HasConversion<string>()
                   .HasDefaultValue(AttemptStatus.Submitted);

            // FK to Subject
            builder.HasOne(e => e.Subject)
                   .WithMany(s => s.Attempts)
                   .HasForeignKey(e => e.SubjectId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.Answers)
                   .WithOne(aa => aa.Attempt)
                   .HasForeignKey(aa => aa.AttemptId)
                   .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(a => a.Student)
                   .WithMany(s => s.Attempts)
                   .HasForeignKey(a => a.StudentId);

            // Indexes for fast queries
            builder.HasIndex(e => e.SubjectId);
          
            builder.HasIndex(e => e.SubmittedAt);
        }
    }
}
