using Domain.Entities;
using Domain.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Persistance.Data;

namespace Presistence.Data
{
    public class DataSeeding : IDataSeeding
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<DataSeeding> _logger;
        private readonly IWebHostEnvironment _env;

        public DataSeeding(
            AppDbContext dbContext,
            ILogger<DataSeeding> logger,
            IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _logger = logger;
            _env = env;
        }

        public async Task SeedDataAsync()
        {
            try
            {
                var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                    await _dbContext.Database.MigrateAsync();

                // Seed Subjects first
                if (!_dbContext.Subjects.Any())
                {
                    await using var subjectData = File.OpenRead("..\\Infrastructure\\Persistance\\Data\\DataSeed\\Subjects.json");
                    var subjects = await JsonSerializer.DeserializeAsync<List<Subject>>(subjectData);
                    if (subjects is not null && subjects.Any())
                    {
                        // ensure ids are zero so DB will generate them
                        foreach (var s in subjects) s.id = 0;
                        await _dbContext.Subjects.AddRangeAsync(subjects);
                        await _dbContext.SaveChangesAsync(); // <-- save now to generate Subject IDs
                    }
                }

                // Then seed Questions
                if (!_dbContext.Questions.Any())
                {
                    await using var questionData = File.OpenRead("..\\Infrastructure\\Persistance\\Data\\DataSeed\\Questions.json");
                    var questions = await JsonSerializer.DeserializeAsync<List<Question>>(questionData);
                    if (questions is not null && questions.Any())
                    {
                        // if questions JSON used subject names instead of ids, map them:
                        // Example: if question JSON has subjectName property you can lookup by name.
                        // But if JSON has subjectId referring to old ids, we need to remap:
                        foreach (var q in questions)
                        {
                            q.id = 0; // let DB generate question id

                            // REMAP subjectId (if original subjectId is present but DB generated different ids)
                            // Best approach: match by a unique field (e.g., subject name). 
                            // If your question JSON contains subjectName, do:
                            // var subj = _dbContext.Subjects.FirstOrDefault(s => s.Name == q.SomeSubjectName);
                            // q.subjectId = subj?.id ?? q.subjectId;

                            // If JSON only has subjectId that matched old ids, you need mapping data to translate old->new ids.
                        }

                        await _dbContext.Questions.AddRangeAsync(questions);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while seeding data");
                throw;
            }
        }

        public Task SeedIdentityDataAsync() => Task.CompletedTask;
    }
}
