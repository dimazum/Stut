using Microsoft.Extensions.DependencyInjection;
using stutvds.Logic.Common;
using stutvds.Logic.Services;
using stutvds.Logic.Services.Contracts;

namespace stutvds.Logic
{
    public static class ServiceCollectionExtension
    {
        public static void AddLogicLayer(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IWikiService, WIkiService>();
            serviceCollection.AddTransient<SyllablesBreaker>();
            serviceCollection.AddScoped<TriggerGeneratorService>();
            serviceCollection.AddScoped<TriggerService>();
            serviceCollection.AddSingleton<ICurrentLanguage, CurrentLanguage>();
        }
    }
}