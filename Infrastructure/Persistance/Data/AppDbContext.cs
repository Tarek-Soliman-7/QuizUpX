using Domain.Entities;
using Domain.Entities.IdentityModule;
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
            modelBuilder.ApplyConfiguration(new AttemptConfiguration());
            modelBuilder.ApplyConfiguration(new StudentConfiguration());



        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Attempt> Attempts { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<AttemptAnswer> AttemptAnswers { get; set; }

    }
}
