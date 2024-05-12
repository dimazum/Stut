using Microsoft.Extensions.DependencyInjection;
using stutvds.Logic.Common;
using stutvds.Logic.Services;
using stutvds.Logic.Services.Contracts;
using stutvds.Logic.Services.Tasks;
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
            serviceCollection.AddTransient<SyllablesBreaker>();
            
            serviceCollection.AddScoped<ITwister, Twister1>();
            serviceCollection.AddScoped<ITwister, Twister2>();
            serviceCollection.AddScoped<ITwister, Twister3>();
            serviceCollection.AddScoped<ITwister, Twister4>();
            serviceCollection.AddScoped<ITwister, Twister5>();
            
            serviceCollection.AddScoped<ITwisterManager, TwisterManager>();
            
            serviceCollection.AddScoped<ITriggerTask, TriggerTask1>();
            serviceCollection.AddScoped<ITriggerTask, TriggerTask2>();
            serviceCollection.AddScoped<ITriggerTask, TriggerTask3>();
            serviceCollection.AddScoped<ITriggerTask, TriggerTask4>();
            
            serviceCollection.AddScoped<TriggerTaskManager>();
        }
    }
}