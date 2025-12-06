using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Data.Configurations
{
    public class AttemptConfiguration : IEntityTypeConfiguration<Attempt>
    {
        public void Configure(EntityTypeBuilder<Attempt> b)
        {
            b.HasKey(x => x.Id);

            b.HasIndex(x => new { x.QuizId, x.StudentId })
             .IsUnique()
             .HasDatabaseName("UQ_Quiz_Student");

            b.Property(x => x.StartedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            b.HasMany(x => x.Answers)
             .WithOne(a => a.Attempt)
             .HasForeignKey(a => a.AttemptId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
