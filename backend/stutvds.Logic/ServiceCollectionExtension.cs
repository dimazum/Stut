using Microsoft.Extensions.DependencyInjection;
using stutvds.Logic.Services;
using stutvds.Logic.Services.Contracts;

namespace stutvds.Logic
{
    public static class ServiceCollectionExtension
    {
        public static void AddLogicLayer(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ITriggerService, TriggerService>();
            serviceCollection.AddTransient<IWikiService, WIkiService>();
        }
    }
}