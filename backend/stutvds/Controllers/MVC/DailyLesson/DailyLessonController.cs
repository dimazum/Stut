using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StopStatAuth_6_0.Entities.Enums;
using stutvds.Controllers.Base;
using stutvds.DAL;
using stutvds.Logic.Services;

namespace stutvds.Controllers.MVC.DailyLesson
{
	//[Authorize(Roles = "Admin, User")]
	public class DailyLessonController : BaseController
	{
		private readonly TriggerRepository _triggerRepository;
		private readonly TriggerService _triggerService;

		public DailyLessonController(TriggerRepository triggerRepository, TriggerService triggerService)
		{
			_triggerRepository = triggerRepository;
			_triggerService = triggerService;
		}
		
		public async Task<IActionResult> Index()
		{
			var triggerVal =  _triggerService.GetRandomTriggers(10);
			
			return View(model: triggerVal);
		}
	}
}
