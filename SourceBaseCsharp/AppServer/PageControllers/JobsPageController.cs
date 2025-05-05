using Microsoft.AspNetCore.Mvc;

namespace AppServer.PageControllers
{
    [Route("tuyen-dung")]
    public class JobsPageController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
