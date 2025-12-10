//using Domain.Entities;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Persistance.Data.Configurations
//{
//    public class ChoiceConfiguration : IEntityTypeConfiguration<Choice>
//    {
//        public void Configure(EntityTypeBuilder<Choice> builder)
//        {
//            builder.ToTable("Choices");

//            builder.HasKey(c => c.Id);

//            builder.Property(c => c.OptionNumber)
//                   .IsRequired();

//            builder.Property(c => c.Text)
//                   .IsRequired()
//                   .HasMaxLength(1000);

//            // prevent duplicate option numbers per question
//            builder.HasIndex(c => new { c.QuestionId, c.OptionNumber }).IsUnique();
//        }
//    }
//}
