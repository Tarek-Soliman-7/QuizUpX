using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Data.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> b)
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.Text)
                .IsRequired();

            b.Property(x => x.Type)
                .IsRequired();

            b.HasMany(x => x.Options)
             .WithOne(o => o.Question)
             .HasForeignKey(o => o.QuestionId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasMany(x => x.Answers)
             .WithOne(a => a.Question)
             .HasForeignKey(a => a.QuestionId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
