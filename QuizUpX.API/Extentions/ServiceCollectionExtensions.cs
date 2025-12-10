using Application.Contracts;
using Core.Services;
using Domain.Contracts;
using Persistance.UnitOfWork;
using Services.Implementations;

namespace QuizUpX.API.Extentions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<ISubjectService, SubjectService>();


            // repositories are created inside UnitOfWork - no need to register them separately
            return services;
        }
    }
}
