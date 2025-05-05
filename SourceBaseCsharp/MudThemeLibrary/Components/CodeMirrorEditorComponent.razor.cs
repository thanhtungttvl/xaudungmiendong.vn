using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MudThemeLibrary.Components
{
    public partial class CodeMirrorEditorComponent : IAsyncDisposable
    {
        private ElementReference _textareaRef;
        private DotNetObjectReference<CodeMirrorEditorComponent>? _dotNetRef;
        private Lazy<Task<IJSObjectReference>> _module = default!;

        [Parameter]
        public string Value { get; set; } = @"<h1>Hello CodeMirror</h1>";

        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }

        [Parameter]
        public string? FileName { get; set; } // để auto detect mode

        [Parameter]
        public int DebounceMilliseconds { get; set; } = 300;

        private CancellationTokenSource? _cts;
        [Parameter]
        public EventCallback<string> OnCodeChanged { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _module = new(() => JsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/MudThemeLibrary/Components/CodeMirrorEditorComponent.razor.js?ttvl-version=1").AsTask());

            // Preload CodeMirror scripts & css trước khi render component
            var module = await _module.Value;
            await module.InvokeVoidAsync("CodeMirrorEditor.preload");

            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var module = await _module.Value;

                _dotNetRef?.Dispose(); // Giải phóng bộ nhớ trước khi tạo mới
                _dotNetRef = DotNetObjectReference.Create(this);

                await module.InvokeVoidAsync("CodeMirrorEditor.init", _textareaRef, _dotNetRef, DetectMode());
            }
        }

        [JSInvokable]
        public async Task OnJsCodeChanged(string newCode)
        {
            _cts?.Cancel(); // hủy debounce cũ nếu có
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            try
            {
                await Task.Delay(DebounceMilliseconds, token);
                if (!token.IsCancellationRequested)
                {
                    Value = newCode;
                    await ValueChanged.InvokeAsync(newCode); // gọi @bind-Value
                    await OnCodeChanged.InvokeAsync(newCode); // gọi sự kiện riêng nếu có
                }
            }
            catch (TaskCanceledException) { }
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

        private string DetectMode()
        {
            if (string.IsNullOrEmpty(FileName)) return "htmlmixed";
            var ext = Path.GetExtension(FileName).ToLowerInvariant();
            return ext switch
            {
                ".js" => "javascript",
                ".ts" => "javascript", // hoặc typescript nếu cậu dùng mode đó
                ".css" => "css",
                ".html" or ".htm" => "htmlmixed",
                ".json" => "application/json",
                ".cs" => "text/x-csharp",
                _ => "htmlmixed"
            };
        }
    }
}
