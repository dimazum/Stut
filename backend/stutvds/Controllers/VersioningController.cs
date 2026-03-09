using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using stutvds.Controllers.Base;

namespace stutvds.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class VersioningController : BaseController
    {
        private readonly IConfiguration _config;

        public VersioningController(IConfiguration config)
        {
            _config = config;
        }
        [HttpGet("hash")]
        public IActionResult GetCurrentCommitHash()
        {
            //from deploy.sh (only live)
            var hash = _config["COMMIT_HASH"] ?? "unknown";

            return Content(hash, "text/plain");
        }
    }
}