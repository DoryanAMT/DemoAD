using System.Diagnostics;
using System.Security.Claims;
using DemoAD.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoAD.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public static UserAzureAD GetUserOnAzureAd(ClaimsPrincipal user)
        {
            var preferredUsernameClaim = user.Claims.FirstOrDefault(c => c.Type.Equals("preferred_username"));
            if (preferredUsernameClaim != null)
            {
                return new UserAzureAD
                {
                    user_object_identifier = user.Claims.FirstOrDefault(p => p.Type.Equals(@"http://schemas.microsoft.com/identity/claims/objectidentifier")).Value,
                    user_name = user.Claims.FirstOrDefault(p => p.Type.Equals("name")).Value,
                    user_email = preferredUsernameClaim.Value,
                    user_domain = string.Format(@"tajamar365\{0}", preferredUsernameClaim.Value.Split('@')[0])
                };
            }
            return null; // O lanzar una excepción si se requiere el reclamo de nombre de usuario preferido
        }
    }
}
