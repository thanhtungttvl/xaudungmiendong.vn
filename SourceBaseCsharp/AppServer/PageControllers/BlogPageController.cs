using Microsoft.AspNetCore.Mvc;

namespace AppServer.PageControllers
{
    [Route("tin-tuc")]
    public class BlogPageController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
