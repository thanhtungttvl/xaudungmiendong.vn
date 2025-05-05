namespace AppAdmin.Layout.Common
{
    public partial class NavMenu : IAsyncDisposable
    {
        protected override async Task OnInitializedAsync()
        {

            await base.OnInitializedAsync();
        }

        public ValueTask DisposeAsync()
        {
            return new ValueTask();
        }
    }
}
