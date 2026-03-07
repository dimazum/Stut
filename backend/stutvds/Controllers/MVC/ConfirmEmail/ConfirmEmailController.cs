using Microsoft.AspNetCore.Mvc;

namespace stutvds.Controllers.MVC.ConfirmEmail
{
    [Route("confirm-email")]
    public class ConfirmEmailController : Controller
    {
        public ConfirmEmailController()
        {
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Confirm email";

            return View();
        }
    }
}