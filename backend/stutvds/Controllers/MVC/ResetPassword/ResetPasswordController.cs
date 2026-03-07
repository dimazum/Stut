using Microsoft.AspNetCore.Mvc;

namespace stutvds.Controllers.MVC.ResetPassword
{
    [Route("reset-password")]
    public class ResetPasswordController : Controller
    {
        public ResetPasswordController()
        {
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Reset password";

            return View();
        }
    }
}