using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistance.Data;
using Shared.Dtos;
using System.Text.Json;

namespace Persistance.Data
{
    public class DataSeeding : IDataSeeding
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<DataSeeding> _logger;
        private readonly IWebHostEnvironment _env;

        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

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
                // Apply pending migrations
                if ((await _dbContext.Database.GetPendingMigrationsAsync()).Any())
                    await _dbContext.Database.MigrateAsync();

                // =========================================
                // ROOT PATH FIX (important part)
                // =========================================
                var seedPath = Path.GetFullPath(
                    Path.Combine(
                        _env.ContentRootPath,
                        "..",
                        "Infrastructure",
                        "Persistance",
                        "Data",
                        "DataSeed"
                    )
                );

                // =============================
                // 1️⃣ Seed Students
                // =============================
                if (!await _dbContext.Students.AnyAsync())
                {
                    var studentsPath = Path.Combine(seedPath, "students.json");

                    await using var studentData = File.OpenRead(studentsPath);
                    var studentsSeed =
                        await JsonSerializer.DeserializeAsync<List<StudentSeedModel>>(studentData, _jsonOptions);

                    if (studentsSeed is not null && studentsSeed.Any())
                    {
                        var students = studentsSeed.Select(s => new Student
                        {
                            Id = 0,
                            Name = s.Name,
                            UniversityCode = s.UniversityCode,
                            Pin = s.Pin,
                            IsActive = true
                        }).ToList();

                        await _dbContext.Students.AddRangeAsync(students);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                // =============================
                // 2️⃣ Seed Subjects
                // =============================
                if (!await _dbContext.Subjects.AnyAsync())
                {
                    var subjectsPath = Path.Combine(seedPath, "Subjects.json");

                    await using var subjectData = File.OpenRead(subjectsPath);
                    var subjects =
                        await JsonSerializer.DeserializeAsync<List<Subject>>(subjectData, _jsonOptions);

                    if (subjects is not null && subjects.Any())
                    {
                        foreach (var s in subjects)
                            s.Id = 0;

                        await _dbContext.Subjects.AddRangeAsync(subjects);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                // =============================
                // 3️⃣ Seed Questions
                // =============================
                if (!await _dbContext.Questions.AnyAsync())
                {
                    var questionsPath = Path.Combine(seedPath, "Questions.json");

                    await using var questionData = File.OpenRead(questionsPath);
                    var questionsSeed =
                        await JsonSerializer.DeserializeAsync<List<Question>>(questionData, _jsonOptions);

                    if (questionsSeed is not null && questionsSeed.Any())
                    {
                        foreach (var q in questionsSeed)
                        {
                            // reset identity
                            q.Id = 0;

                            // safety check (important)
                            if (q.Choices == null)
                                q.Choices = new List<string>();

                            // OPTIONAL: validate subjectId exists
                            var subjectExists = await _dbContext.Subjects
                                .AnyAsync(s => s.Id == q.SubjectId);

                            if (!subjectExists)
                            {
                                _logger.LogWarning(
                                    $"Question '{q.Title}' skipped. SubjectId '{q.SubjectId}' not found."
                                );
                                continue;
                            }
                        }

                        await _dbContext.Questions.AddRangeAsync(questionsSeed);
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
    }
}
