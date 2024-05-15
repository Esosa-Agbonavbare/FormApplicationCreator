using FormApplicationCreator.Application.Services.Implementation;
using FormApplicationCreator.Application.Services.Interface;
using FormApplicationCreator.Persistence;
using FormApplicationCreator.Persistence.Repositories.Implementation;
using FormApplicationCreator.Persistence.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace FormApplicationCreator.API.Extensions
{
    public static class DIServiceExtension
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var cosmosServiceUri = configuration["CosmosDb:ServiceUri"];
            var cosmosAuthKey = configuration["CosmosDb:AuthKey"];
            var cosmosDatabaseId = configuration["CosmosDb:DatabaseId"];

            services.AddDbContext<MyDbContext>(options =>
            {
                options.UseCosmos(cosmosServiceUri, cosmosAuthKey, databaseName: cosmosDatabaseId);
            });
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<ICandidateService, CandidateService>();
            services.AddScoped<ICandidateRepository, CandidateRepository>();
            services.AddScoped<IResponseRepository, ResponseRepository>();
            services.AddScoped<IApplicationFormService, ApplicationFormService>();
            services.AddScoped<IApplicationFormRepository, ApplicationFormRepository>();
        }
    }
}
