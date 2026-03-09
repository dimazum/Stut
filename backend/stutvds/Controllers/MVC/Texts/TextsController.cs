using Microsoft.AspNetCore.Mvc;

namespace stutvds.Controllers
{
	public class TextsController : Controller
	{
		public IActionResult Index()
		{
			ViewData["Title"] = "Случайный текст для чтения вслух — тренировка речи онлайн";
			ViewData["Description"] = "Случайный текст для чтения вслух онлайн. Улучшайте дикцию, темп и уверенность речи с бесплатными упражнениями.";
			ViewData["Keywords"] = "текст для чтения вслух, дикция, тренировка речи, упражнения для речи";
			
			return View();
		}
	}
}
