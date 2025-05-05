using Microsoft.AspNetCore.Mvc;

namespace AppServer.PageControllers
{
    [Route("cau-hoi-thuong-gap")]
    public class FaqPageController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
