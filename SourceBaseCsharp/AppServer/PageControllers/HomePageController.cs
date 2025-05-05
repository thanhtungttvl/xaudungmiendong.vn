using Microsoft.AspNetCore.Mvc;

namespace AppServer.PageControllers
{
    [Route("/")]
    public class HomePageController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
