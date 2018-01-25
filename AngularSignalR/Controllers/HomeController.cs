using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using WebSockets.UserTracker;

namespace AngularSignalR.Controllers
{
    public class HomeController : Controller
    {
        private IAuthenticator _authenticator;
        
        public HomeController(IAuthenticator auth)
        {
            _authenticator = auth;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> TrySignin()
        {
            var guidData = await _authenticator.AddUserToWaiting(Request.Query["username"], Request.Query["roomname"]);
            return Json(new { guid = guidData });
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }
    }
}
