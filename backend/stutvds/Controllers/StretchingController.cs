using Microsoft.AspNetCore.Mvc;
using stutvds.Controllers.Base;
using stutvds.Logic.Services.Contracts;

namespace stutvds.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StretchingController : BaseController
    {
        private readonly IStretchingService _stretchingService;

        public StretchingController(IStretchingService stretchingService)
        {
            _stretchingService = stretchingService;
        }
        
        [HttpGet]
        public JsonResult Get()
        {
            var content = _stretchingService.GetAll();
            return new JsonResult(content);
        }
    }
}