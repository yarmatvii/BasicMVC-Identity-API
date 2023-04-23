using _7semester_ASP_SecondTask.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;

namespace _7semester_ASP_SecondTask.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IUserStore<IdentityUser> _userStore;
		private readonly IUserEmailStore<IdentityUser> _emailStore;
		private readonly SignInManager<IdentityUser> _signInManager;

		public HomeController(ILogger<HomeController> logger,
			UserManager<IdentityUser> userManager,
			RoleManager<IdentityRole> roleManager,
			IUserStore<IdentityUser> userStore,
		SignInManager<IdentityUser> signInManager)
		{
			_logger = logger;
			_userManager = userManager;
			_roleManager = roleManager;
			_userStore = userStore;
			_emailStore = GetEmailStore();
			_signInManager = signInManager;
		}

		public IActionResult Index()
		{
			return View();
		}

		[Authorize(Roles = "adminRole")]
		public IActionResult Swagger()
		{
			return Redirect("~/swagger/index.html");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		//public async Task<IActionResult> CreateUser(string e, string p)
		//{
		//	var user = Activator.CreateInstance<IdentityUser>();

		//	await _userStore.SetUserNameAsync(user, e, CancellationToken.None);
		//	await _emailStore.SetEmailAsync(user, e, CancellationToken.None);
		//	await _userManager.CreateAsync(user, p);
		//	_userManager.Options.SignIn.RequireConfirmedAccount = false;
		//	return RedirectToAction("Index", "Home");
		//}

		//public async Task<IActionResult> CheckRole(string role)
		//{
		//	var userName = HttpContext.User.Identity.Name;
		//	var user = await _userManager.FindByNameAsync(userName);
		//	//await _userManager.AddToRoleAsync(user, role);
		//	var check = await _userManager.IsInRoleAsync(user, role);
		//	if (check) ViewData["check"] = "true for found user";
		//	else ViewData["check"] = "false for found user";
		//	var check2 = HttpContext.User.IsInRole(role);
		//	if (check2) ViewData["check2"] = "true for user";
		//	else ViewData["check2"] = "false for user";
		//	return View();
		//}

		private IUserEmailStore<IdentityUser> GetEmailStore()
		{
			if (!_userManager.SupportsUserEmail)
			{
				throw new NotSupportedException("The default UI requires a user store with email support.");
			}
			return (IUserEmailStore<IdentityUser>)_userStore;
		}

		[Authorize]
		public async Task<IActionResult> GiveRole()
		{
			var role = Activator.CreateInstance<IdentityRole>();
			role.NormalizedName = "ADMINROLE";
			role.Name = "adminRole";
			role.Id = "1";
			role.ConcurrencyStamp = Guid.NewGuid().ToString();
			await _roleManager.CreateAsync(role);
			var userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			await _userManager.AddToRoleAsync(user, "adminRole");
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
	}
}