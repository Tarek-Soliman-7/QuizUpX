using Microsoft.EntityFrameworkCore;
using Persistance.Data;
using Domain.Contracts;
using Presistence.Data;
using QuizUpX.API.Extentions;
using Microsoft.AspNetCore.Identity;

namespace QuizUpX.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ---------------------------------------------------------
            // 1) Register DbContext
            // ---------------------------------------------------------
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                )
            );

            // ---------------------------------------------------------
            // 2) Register Application Services (Repositories + Services)
            //    This comes from your ServiceCollectionExtensions class
            // ---------------------------------------------------------
            builder.Services.AddApplicationServices();

            // ---------------------------------------------------------
            // 3) Register Data Seeding
            // ---------------------------------------------------------
            builder.Services.AddScoped<IDataSeeding, DataSeeding>();
            builder.Services.AddSingleton<IPasswordHasher<string>, PasswordHasher<string>>();
            // ---------------------------------------------------------
            // 4) Add framework services
            // ---------------------------------------------------------
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
           

            var app = builder.Build();

            // ---------------------------------------------------------
            // 5) Run Data Seeding INSIDE a scope
            // ---------------------------------------------------------
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var seeder = services.GetRequiredService<IDataSeeding>();
                    await seeder.SeedDataAsync();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
          

 

            app.UseCors("AllowAll");

            // ---------------------------------------------------------
            // 6) Configure HTTP Pipeline
            // ---------------------------------------------------------
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            await app.RunAsync();
        }
    }
}
