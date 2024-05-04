using Microsoft.AspNetCore.Mvc;
using stutvds.Controllers.Base;

namespace stutvds.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TwistersController: BaseController
    {
        public TwistersController()
        {
            
        }

        [HttpGet]
        public JsonResult GetTwisters()
        {
            
        }
    }
}