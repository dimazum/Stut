using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace stutvds.Controllers
{
	[Authorize(Roles = "Admin, User")]
	public class LinkedAccountsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
