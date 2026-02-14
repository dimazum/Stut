using Microsoft.AspNetCore.Mvc;

namespace stutvds.Controllers
{
	public class BreathingController : Controller
	{
		public IActionResult Index()
		{
			ViewData["Title"] = "Правильное дыхание при заикании";
			ViewData["Description"] = "Упражнения для дыхания";
			ViewData["Keywords"] = "упражнения для дыхания при заикании, дыхание, тренировка дыхания";
			
			return View();
		}
	}
}
