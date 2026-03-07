using System.Threading.Tasks;
using RazorLight;

namespace stutvds.Emails;

public interface IRazorEmailRenderer
{
    Task<string> RenderAsync<T>(string templateName, T model);
}

public class RazorEmailRenderer : IRazorEmailRenderer
{
    private readonly RazorLightEngine _engine;

    public RazorEmailRenderer()
    {
        _engine = new RazorLightEngineBuilder()
            .UseEmbeddedResourcesProject(typeof(RazorEmailRenderer).Assembly)
            .EnableDebugMode()
            .UseMemoryCachingProvider()
            .Build();
    }
    
    public Task<string> RenderAsync<T>(string templateName, T model)
    {
        return _engine.CompileRenderAsync(templateName, model);
    }
}