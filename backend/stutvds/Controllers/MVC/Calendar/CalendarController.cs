using Microsoft.AspNetCore.Mvc;

namespace stutvds.Controllers.MVC.Calendar
{
	public class CalendarController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
