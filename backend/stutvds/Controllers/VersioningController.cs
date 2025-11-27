using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using stutvds.Controllers.Base;

namespace stutvds.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class VersioningController : BaseController
    {
        [HttpGet]
        [Route("current")]
        public JsonResult GetCurrentVersion()
        {
            var entryAssembly = Assembly.GetEntryAssembly();

            var version = entryAssembly?
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion;

            return new JsonResult(version);
        }
    }
}