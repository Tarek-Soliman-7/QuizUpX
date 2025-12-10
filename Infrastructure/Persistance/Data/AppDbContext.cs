using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistance.Data.Configurations;

namespace Persistance.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfiguration(new ChoiceConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionConfiguration());



        }

        public DbSet<Question> Questions { get; set; }
        //public DbSet<Choice> Choices { get; set; }
        public DbSet<Subject> Subjects { get; set; }
       

    }
}
