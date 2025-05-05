using Microsoft.AspNetCore.Mvc;

namespace AppServer.PageControllers
{
    [Route("thu-vien")]
    public class GalleryPageController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
