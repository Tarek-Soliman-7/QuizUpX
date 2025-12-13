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
            services.AddScoped<IStudentRepository, StudentRepository>();

            services.AddScoped<IQuizService, QuizService>();





            // repositories are created inside UnitOfWork - no need to register them separately
            return services;
        }
    }
}
