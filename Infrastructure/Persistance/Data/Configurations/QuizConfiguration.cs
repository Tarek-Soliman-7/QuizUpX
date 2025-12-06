using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Data.Configurations
{
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> b)
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(300);

            b.Property(x => x.IsPublished)
                .HasDefaultValue(false);

            b.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            b.HasMany(x => x.Questions)
             .WithOne(q => q.Quiz)
             .HasForeignKey(q => q.QuizId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasMany(x => x.Attempts)
             .WithOne(a => a.Quiz)
             .HasForeignKey(a => a.QuizId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
