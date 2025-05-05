using Microsoft.AspNetCore.Mvc;

namespace AppServer.PageControllers.Shared.Components
{
    public class MainFooterViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult(View("~/Views/Shared/ViewComponents/_MainFooter.cshtml"));
        }
    }
}
