using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Data.Configurations
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> b)
        {
            b.HasKey(x => x.Id);

            b.HasOne(x => x.SelectedOption)
             .WithMany()
             .HasForeignKey(x => x.SelectedOptionId)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasIndex(x => x.AttemptId);
            b.HasIndex(x => x.QuestionId);
        }
    }
}
