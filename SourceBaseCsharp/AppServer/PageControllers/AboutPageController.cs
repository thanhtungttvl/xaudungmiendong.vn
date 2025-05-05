using Microsoft.AspNetCore.Mvc;

namespace AppServer.PageControllers
{
    [Route("gioi-thieu")]
    public class AboutPageController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("lich-su")]
        public IActionResult History()
        {
            return View();
        }

        [HttpGet]
        [Route("tuyen-ngon")]
        public IActionResult Declaration()
        {
            return View();
        }

        [HttpGet]
        [Route("thanh-tich")]
        public IActionResult Achievements()
        {
            return View();
        }
    }
}
