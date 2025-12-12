using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Persistance.Data.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Questions");

            builder.HasKey(q => q.id);

            builder.Property(q=>q.id).UseIdentityColumn();

            builder.Property(q => q.title)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(q => q.questionType)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(q => q.mark)
                   .IsRequired()
                   .HasDefaultValue(1);

            builder.Property(q => q.correctIndex)
                   .IsRequired()
                   .HasDefaultValue(0);

            builder.HasIndex(q => new { q.subjectId });

            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            builder.Property(q => q.choices)
                   .HasConversion(
                       v => JsonSerializer.Serialize(v, jsonOptions),
                       v => string.IsNullOrWhiteSpace(v)
                            ? new List<string>()
                            : JsonSerializer.Deserialize<List<string>>(v, jsonOptions) ?? new List<string>()
                   )
                   .HasColumnType("nvarchar(max)");


        }
    }
}
