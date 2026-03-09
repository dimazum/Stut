using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SpeechApp.Models;

namespace stutvds.Controllers
{
	//[Authorize(Roles = "Admin, User")]
	public class ExercisesController : Controller
	{
		public IActionResult Index()
		{
			var categories = new List<ArticulationCategory>
			{
				LoadJson("lip-articulation.json"),
				//LoadJson("front-tongue.json"),
				//LoadJson("back-tongue.json")
			};

			return View(categories);
		}

		private ArticulationCategory LoadJson(string fileName)
		{
			var path = Path.Combine(
				Directory.GetCurrentDirectory(),
				"wwwroot/data",
				fileName
			);

			var json = System.IO.File.ReadAllText(path);

			return JsonSerializer.Deserialize<ArticulationCategory>(
				json,
				new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
			);
		}

		
		
		
		
	}
}
