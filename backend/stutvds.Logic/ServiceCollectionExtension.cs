using Microsoft.Extensions.DependencyInjection;
using stutvds.Logic.Services;
using stutvds.Logic.Services.Contracts;
using stutvds.Logic.Twisters;

namespace stutvds.Logic
{
    public static class ServiceCollectionExtension
    {
        public static void AddLogicLayer(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ITriggerService, TriggerService>();
            serviceCollection.AddTransient<IWikiService, WIkiService>();
            serviceCollection.AddTransient<IStretchingService, StretchingService>();
            
            serviceCollection.AddScoped<ITwister, Twister1>();
            serviceCollection.AddScoped<ITwister, Twister2>();
            serviceCollection.AddScoped<ITwister, Twister3>();
            serviceCollection.AddScoped<ITwister, Twister4>();
            serviceCollection.AddScoped<ITwister, Twister5>();
            
            serviceCollection.AddScoped<ITwisterManager, TwisterManager>();
        }
    }
}