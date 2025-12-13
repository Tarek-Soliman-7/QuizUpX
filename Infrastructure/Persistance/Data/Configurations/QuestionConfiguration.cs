using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace Persistance.Data.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Questions");

            builder.HasKey(q => q.Id);

            builder.Property(q => q.Id)
                   .UseIdentityColumn();

            builder.Property(q => q.Title)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(q => q.QuestionType)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(q => q.Mark)
                   .IsRequired()
                   .HasDefaultValue(1);

            builder.Property(q => q.CorrectIndex)
                   .IsRequired()
                   .HasDefaultValue(0);

            builder.HasIndex(q => q.SubjectId);

            // =========================
            // Choices (List<string>)
            // =========================
            builder.Property(q => q.Choices)
                   .HasColumnType("nvarchar(max)")
                   .HasConversion(
                       v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                       v => string.IsNullOrWhiteSpace(v)
                            ? new List<string>()
                            : JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null)!
                   )
                   .Metadata.SetValueComparer(
                       new ValueComparer<List<string>>(
                           (c1, c2) => c1.SequenceEqual(c2),
                           c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                           c => c.ToList()
                       )
                   );

            // =========================
            // Relationships
            // =========================
            builder.HasMany(q => q.AttemptAnswers)
                   .WithOne(a => a.Question)
                   .HasForeignKey(a => a.QuestionId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
