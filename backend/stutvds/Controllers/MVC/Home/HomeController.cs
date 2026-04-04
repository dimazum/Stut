using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using stutvds.Models;
using System.Diagnostics;
using System.Globalization;

namespace stutvds.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			ViewData["offStuContainer"] = true;
			var userLang = Request.Headers["Accept-Language"].ToString();

			if (!string.IsNullOrEmpty(userLang) && userLang.StartsWith("en", System.StringComparison.OrdinalIgnoreCase))
			{
				return Redirect("/en");
			}
			
			return View();
		}
		
		[Route("en")]
		public IActionResult English()
		{
			ViewData["offStuContainer"] = true;
			return View("~/Controllers/MVC/Home/en/Index.cshtml");
		}
	}
}
