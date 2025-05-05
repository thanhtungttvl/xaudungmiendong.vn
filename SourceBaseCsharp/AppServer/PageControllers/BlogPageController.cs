using Microsoft.AspNetCore.Mvc;

namespace AppServer.PageControllers
{
    [Route("blog")]
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
