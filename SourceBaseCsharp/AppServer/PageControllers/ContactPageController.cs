using Microsoft.AspNetCore.Mvc;

namespace AppServer.PageControllers
{
    [Route("lien-he")]
    public class ContactPageController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
