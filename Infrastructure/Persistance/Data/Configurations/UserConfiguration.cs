using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Data.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> b)
        {
            b.HasKey(x => x.Id);

            b.HasIndex(x => x.UniversityCode).IsUnique();

            b.Property(x => x.UniversityCode)
                .IsRequired()
                .HasMaxLength(50);

            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            b.Property(x => x.Role)
                .IsRequired();

            b.Property(x => x.Password).IsRequired();

            b.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            b.HasMany(x => x.Quizzes)
             .WithOne(q => q.Doctor)
             .HasForeignKey(q => q.DoctorId)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasMany(x => x.Attempts)
             .WithOne(a => a.Student)
             .HasForeignKey(a => a.StudentId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
