using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace MudThemeLibrary.Components
{
    public partial class CropperjsComponent : IAsyncDisposable
    {
        private DotNetObjectReference<CropperjsComponent>? _dotNetRef;
        private Lazy<Task<IJSObjectReference>> _module = default!;
        private IJSObjectReference _cropperRef = default!;
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
        [Parameter] public string Base64WithPrefix { get; set; } = default!;

        private ElementReference _imageRef = default!;

        private string _dataUrl { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            _module = new(() => JsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/MudThemeLibrary/Components/CropperjsComponent.razor.js?ttvl-version=1").AsTask());

            // Preload Cropperjs scripts & css trước khi render component
            var module = await _module.Value;
            await module.InvokeVoidAsync("Cropperjs.preload");

            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var module = await _module.Value;
                _cropperRef = await module.InvokeAsync<IJSObjectReference>("Cropperjs.init", _imageRef);
            }
        }

        public async Task OnCroppedCanvas()
        {
            var module = await _module.Value;
            _dataUrl = await module.InvokeAsync<string>("Cropperjs.croppedCanvas", _cropperRef);
            StateHasChanged();
        }

        public async Task OnRotate(int option)
        {
            var module = await _module.Value;
            _cropperRef = await module.InvokeAsync<IJSObjectReference>("Cropperjs.rotate", _cropperRef, option);
        }

        private int _scaleX { get; set; } = -1;
        public async Task OnScaleX()
        {
            var module = await _module.Value;
            _cropperRef = await module.InvokeAsync<IJSObjectReference>("Cropperjs.scaleX", _cropperRef, _scaleX);
            _scaleX = -_scaleX;
        }

        private int _scaleY { get; set; } = -1;
        public async Task OnScaleY()
        {
            var module = await _module.Value;
            _cropperRef = await module.InvokeAsync<IJSObjectReference>("Cropperjs.scaleY", _cropperRef, _scaleY);
            _scaleY = -_scaleY;
        }

        public async ValueTask DisposeAsync()
        {
            _dotNetRef?.Dispose();
            if (_module.IsValueCreated)
            {
                var module = await _module.Value;
                await module.DisposeAsync();
            }
        }

        private async Task Submit()
        {
            var module = await _module.Value;
            var data = await module.InvokeAsync<string>("Cropperjs.croppedCanvas", _cropperRef);
            MudDialog.Close(DialogResult.Ok(data));
        }

        private void Cancel() => MudDialog.Cancel();
    }
}
