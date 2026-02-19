using Microsoft.AspNetCore.Mvc;
using stutvds.Logic.Services;

namespace stutvds.Controllers.MVC.PracticeWord
{
    public class PracticeWordController : Controller
    {
        private readonly TriggerService _triggerService;

        public PracticeWordController(TriggerService triggerService)
        {
            _triggerService = triggerService;
        }
        public IActionResult Index(string trigger = null)
        {
            ViewData["Title"] = "Тренировка слова";
            ViewData["Description"] = "Тренируем слово, которое вызывает запинку/блок";
            ViewData["Keywords"] = "лечение блоков при заикании, лечение заикания, тренировка речи";
        
            if (string.IsNullOrEmpty(trigger))
            {
                trigger = _triggerService.GetRandomTriggers();
            }
        
            return View(model: trigger);
        }
    }
}