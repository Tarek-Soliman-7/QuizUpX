using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Data.Configurations
{
    public class OptionConfiguration : IEntityTypeConfiguration<Option>
    {
        public void Configure(EntityTypeBuilder<Option> b)
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.Text)
                .IsRequired()
                .HasMaxLength(500);

            b.Property(x => x.IsCorrect)
                .IsRequired();
        }
    }
}
