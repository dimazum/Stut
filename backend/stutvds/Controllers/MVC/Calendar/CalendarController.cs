using Microsoft.AspNetCore.Mvc;

namespace stutvds.Controllers
{
	public class CalendarController : Controller
	{

		public CalendarController()
		{
		}

		public IActionResult Index()
		{
			ViewData["Title"] = "Календарь прогресса речи — отслеживайте тренировки";
			ViewData["Description"] = "Отслеживайте свой прогресс в упражнениях для дикции, дыхания и интонации с помощью календаря тренировок.";
			ViewData["Keywords"] = "календарь, прогресс речи, упражнения, тренировка дикции, упражнения для голоса";

			
			return View();
		}
	}
}
