using Microsoft.Extensions.DependencyInjection;
using stutvds.DAL.Repositories;
using stutvds.DAL.Repositories.Contracts;

namespace stutvds.DAL
{
    public static class ServiceCollectionExtension
    {
        public static void AddDataAccessLayer(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ITriggerRepository, TriggerRepository>();
            serviceCollection.AddScoped<IArticleRepository, ArticleRepository>();
            serviceCollection.AddScoped<DayLessonRepository>();
        }
    }
}