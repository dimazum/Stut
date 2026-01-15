using Microsoft.AspNetCore.Mvc;

namespace stutvds.Controllers
{
	//[Authorize(Roles = "Admin, User")]
	public class DailyLessonController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
