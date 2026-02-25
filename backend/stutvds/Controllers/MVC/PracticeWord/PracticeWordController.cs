using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using stutvds.Controllers.Base;
using stutvds.DAL;

namespace stutvds.Controllers.MVC.PracticeWord
{
    public class PracticeWordController : BaseController
    {
        private TriggerRepository _triggerRepository;

        public PracticeWordController(TriggerRepository triggerRepository)
        {
            _triggerRepository = triggerRepository;
        }
        public async Task<IActionResult> Index(string trigger = null)
        {
            ViewData["Title"] = "Тренировка слова";
            ViewData["Description"] = "Тренируем слово, которое вызывает запинку/блок";
            ViewData["Keywords"] = "лечение блоков при заикании, лечение заикания, тренировка речи";
        
            if (string.IsNullOrEmpty(trigger))
            {
                //trigger = (await _triggerRepository.GetFirstTrigger(UserId, CurrentLanguage)).Value;
            }
        
            return View(model: trigger);
        }
    }
}