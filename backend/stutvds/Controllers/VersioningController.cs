using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using stutvds.Controllers.Base;

namespace stutvds.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class VersioningController : BaseController
    {
        [HttpGet("current")]
        public IActionResult GetCurrentVersion()
        {
            var entryAssembly = Assembly.GetEntryAssembly();

            var version = entryAssembly?
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion;

            var cleanVersion = version?.Split('+')[0];

            return Content(cleanVersion!, "text/plain");
        }
    }
}