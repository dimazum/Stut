using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace stutvds.Controllers
{
	[Authorize(Roles = "Admin, User")]
	public class AdminController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
