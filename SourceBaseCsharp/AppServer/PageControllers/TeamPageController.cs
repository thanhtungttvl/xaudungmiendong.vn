using Microsoft.AspNetCore.Mvc;

namespace AppServer.PageControllers
{
    [Route("team")]
    public class TeamPageController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
