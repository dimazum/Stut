using Microsoft.Extensions.DependencyInjection;
using stutvds.DAL.Repositories;
using stutvds.DAL.Repositories.Contracts;

namespace stutvds.DAL
{
    public static class ServiceCollectionExtension
    {
        public static void AddDataAccessLayer(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<TriggerRepository>();
            serviceCollection.AddScoped<IArticleRepository, ArticleRepository>();
            serviceCollection.AddScoped<DayLessonRepository>();
            serviceCollection.AddScoped<VoiceAnalyseRepository>();
            serviceCollection.AddScoped<HistogramRepository>();
            serviceCollection.AddScoped<UserRepository>();
            serviceCollection.AddScoped<IStoredProcedureInstaller, StoredProcedureInstaller>();
            serviceCollection.AddScoped<ChatMessageRepository>();
        }
    }
}