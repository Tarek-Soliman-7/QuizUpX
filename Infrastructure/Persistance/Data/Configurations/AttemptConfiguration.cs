using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Data.Configurations
{
    public class AttemptConfiguration : IEntityTypeConfiguration<Attempt>
    {
        public void Configure(EntityTypeBuilder<Attempt> builder)
        {
            builder.ToTable("Attempts");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.AnswersJson)
                .IsRequired()
                   .HasColumnType("nvarchar(max)");

            builder.Property(e => e.Status)
                   .HasMaxLength(50)
                   .HasDefaultValue("Submitted");

            // FK to Subject
            builder.HasOne(e => e.Subject)
                   .WithMany(s => s.Attempts)
                   .HasForeignKey(e => e.SubjectId)
                   .OnDelete(DeleteBehavior.Cascade);

            // If you have ApplicationUser navigation, configure it here. If not, leave UserId as string.
            // Example (uncomment & adjust if you have ApplicationUser):
            // builder.HasOne<Domain.Identity.ApplicationUser>()
            //        .WithMany()
            //        .HasForeignKey(e => e.UserId)
            //        .OnDelete(DeleteBehavior.SetNull);

            // Indexes for fast queries
            builder.HasIndex(e => e.SubjectId);
            builder.HasIndex(e => e.UserId);
            builder.HasIndex(e => e.SubmittedAt);
        }
    }
}
