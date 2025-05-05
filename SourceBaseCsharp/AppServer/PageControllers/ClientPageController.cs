using Microsoft.AspNetCore.Mvc;

namespace AppServer.PageControllers
{
    [Route("khach-hang-doi-tac")]
    public class ClientPageController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
