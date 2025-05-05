using Microsoft.AspNetCore.Components;

namespace MudThemeLibrary.Theme
{
    public partial class LoadingTheme
    {
        private bool _visible = true;

        [Parameter]
        public bool Visible
        {
            get => _visible;
            set
            {
                if (_visible != value)
                {
                    _visible = value;
                    OpenChanged.InvokeAsync(value); // Gọi sự kiện khi giá trị thay đổi
                }
            }
        }

        [Parameter]
        public EventCallback<bool> OpenChanged { get; set; }
    }
}
