using Application.Contracts;
using Core.Services;
using Domain.Contracts;
using Persistance.Repositories;
using Persistance.UnitOfWork;
using Services.Abstraction.Contracts;
using Services.Implementations;

namespace QuizUpX.API.Extentions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IQuestionRepository,QuestionRepository>();
            services.AddScoped<ISubjectRepository,SubjectRepository>();
            services.AddScoped<IAttemptRepository, AttemptRepository>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<IAttemptService, AttemptService>();
            services.AddScoped<IStudentRepository, StudentRepository>();

            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IExamService, ExamService>();





            // repositories are created inside UnitOfWork - no need to register them separately
            return services;
        }
    }
}
