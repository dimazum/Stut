using Microsoft.AspNetCore.Mvc;

namespace stutvds.Controllers
{
	public class AboutProjectController : Controller
	{

		public AboutProjectController()
		{
		}

		public IActionResult Index()
		{
			ViewData["Title"] = "О проекте";
			ViewData["Description"] = "О проекте для лечения заикания";
			ViewData["Keywords"] = "О проекте для лечения заикания";

			
			return View();
		}
	}
}
