using Microsoft.AspNetCore.Mvc;

namespace AppServer.PageControllers.Shared.Components
{
    public class MainHeaderViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult(View("~/Views/Shared/ViewComponents/_MainHeader.cshtml"));
        }
    }
}
