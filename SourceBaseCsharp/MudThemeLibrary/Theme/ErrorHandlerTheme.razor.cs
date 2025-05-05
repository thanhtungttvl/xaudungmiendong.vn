using Microsoft.AspNetCore.Components;

namespace MudThemeLibrary.Theme
{
    public partial class ErrorHandlerTheme
    {
        [Parameter] public string? JsonUrl { get; set; } = default!;
        [Parameter] public string? Class { get; set; } = "w-50 mx-auto";
        [Parameter] public string? Style { get; set; } = string.Empty;
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? AdditionalAttributes { get; set; }

        private Dictionary<string, object> _mergedAttributes
        {
            get
            {
                var attrs = new Dictionary<string, object>(AdditionalAttributes ?? []);

                if (!string.IsNullOrWhiteSpace(Style))
                    attrs["style"] = Style;

                if (!string.IsNullOrWhiteSpace(Class))
                    attrs["class"] = Class;

                return attrs;
            }
        }
    }
}
