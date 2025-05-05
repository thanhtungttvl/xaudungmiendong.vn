using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MudThemeLibrary.Theme
{
    public partial class LottieSVGTheme : IAsyncDisposable
    {
        /// <summary>
        /// Đường dẫn tới file Lottie JSON. Có thể là đường dẫn nội bộ hoặc URL.
        /// </summary>
        [Parameter] public string? JsonUrl { get; set; }

        /// <summary>
        /// Chiều rộng của animation (ví dụ: "200px", "100%").
        /// </summary>
        [Parameter] public string? Width { get; set; }

        /// <summary>
        /// CSS style tùy chỉnh (inline style).
        /// </summary>
        [Parameter] public string? Style { get; set; }

        /// <summary>
        /// Tên lớp CSS áp dụng cho thẻ div chứa animation.
        /// </summary>
        [Parameter] public string? Class { get; set; }

        /// <summary>
        /// Nếu true, đường dẫn JsonUrl sẽ được thêm tiền tố từ config server.
        /// </summary>
        [Parameter] public bool IsFromServer { get; set; } = true;

        /// <summary>
        /// Có lặp lại animation hay không.
        /// </summary>
        [Parameter] public bool Loop { get; set; } = true;

        /// <summary>
        /// Tốc độ chạy animation (1 là bình thường, 2 là nhanh gấp đôi).
        /// </summary>
        [Parameter] public double Speed { get; set; } = 1;

        /// <summary>
        /// Các thuộc tính HTML bổ sung không xác định rõ trong component.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? AdditionalAttributes { get; set; }

        private ElementReference _refLottieSvg;

        private Lazy<Task<IJSObjectReference>> _module = default!;

        /// <summary>
        /// Trộn các thuộc tính style, class và các attribute khác để áp dụng vào thẻ div.
        /// </summary>
        private Dictionary<string, object> _mergedAttributes
        {
            get
            {
                var attrs = new Dictionary<string, object>(AdditionalAttributes ?? []);

                var combinedStyle = new List<string>();
                if (!string.IsNullOrWhiteSpace(Style)) combinedStyle.Add(Style!.TrimEnd(';'));
                if (!string.IsNullOrWhiteSpace(Width)) combinedStyle.Add($"width: {Width}");

                if (combinedStyle.Count > 0)
                    attrs["style"] = string.Join("; ", combinedStyle) + ";";

                if (!string.IsNullOrWhiteSpace(Class))
                    attrs.TryAdd("class", Class);

                return attrs;
            }
        }

        /// <summary>
        /// Import JS module khi component khởi tạo.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            _module = new(() =>
                JsRuntime.InvokeAsync<IJSObjectReference>(
                    "import", "./_content/MudThemeLibrary/Theme/LottieSVGTheme.razor.js?ttvl-version=1"
                ).AsTask()
            );

            var module = await _module.Value;
            await module.InvokeVoidAsync("LottieSVG.preload");

            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Khởi chạy animation sau khi component được render lần đầu.
        /// </summary>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender) return;

            var serverUrl = ConfigService.GetValue("Servers:Default:Url");
            var jsonPath = IsFromServer ? $"{serverUrl}/{JsonUrl}" : "https://raw.githubusercontent.com/tungnguyenvanthanh/lottie-files/main/" + JsonUrl;

            var module = await _module.Value;
            await module.InvokeVoidAsync("LottieSVG.init", _refLottieSvg, jsonPath, Loop, Speed);
        }

        /// <summary>
        /// Giải phóng JS module khi component bị huỷ.
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            if (_module.IsValueCreated)
            {
                var module = await _module.Value;
                await module.DisposeAsync();
            }
        }
    }
}
