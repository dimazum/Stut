using System.Linq;
using Microsoft.AspNetCore.Mvc;
using stutvds.Controllers.Base;
using stutvds.Logic.Services.Contracts;

namespace stutvds.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TwistersController: BaseController
    {
        private readonly ITwisterManager _twisterManager;

        public TwistersController(ITwisterManager twisterManager)
        {
            _twisterManager = twisterManager;
        }

        [HttpGet]
        public JsonResult GetTwisters()
        {
            return new JsonResult(_twisterManager.GetAllTwisters());
        }
    }
}