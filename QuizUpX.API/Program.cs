using Microsoft.EntityFrameworkCore;
using Persistance.Data;
using Persistance.Repositories;
using Persistance.UnitOfWork;
using Domain.Contracts;
using Services.Abstraction.Contracts;
using Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

#region Add services

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// =======================
// Dependency Injection
// =======================

// Repositories
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IAttemptRepository, AttemptRepository>();
builder.Services.AddScoped<IAttemptAnswerRepository, AttemptAnswerRepository>();

// Unit Of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Services
builder.Services.AddScoped<IQuizService, QuizService>();

builder.Services.AddScoped<IDataSeeding, DataSeeding>();


// CORS (Flutter)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFlutter", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

#endregion

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeding>();
    await seeder.SeedDataAsync();
}
#region Middleware pipeline

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS
app.UseHttpsRedirection();

// CORS
app.UseCors("AllowFlutter");

// Authorization (حتى لو مش مستخدم Auth)
app.UseAuthorization();

// Controllers
app.MapControllers();

#endregion

app.Run();
