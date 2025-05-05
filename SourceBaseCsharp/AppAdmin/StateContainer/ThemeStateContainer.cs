using MudThemeLibrary.Handlers;

namespace AppAdmin.StateContainer
{
    public class ThemeStateContainer
    {
        public event Action? OnStateChange;
        private void NotifyStateChanged() => OnStateChange?.Invoke();
        private readonly SecureLocalStorageHandler _secureLocalStorage;

        public ThemeStateContainer(SecureLocalStorageHandler secureLocalStorage)
        {
            _secureLocalStorage = secureLocalStorage;
        }

        private bool _isDarkMode;
        public bool IsDarkMode
        {
            get => _isDarkMode;
            set
            {
                if (_isDarkMode != value)
                {
                    _ = _secureLocalStorage.SetItemAsync("dark-mode", value);
                    _isDarkMode = value;
                    NotifyStateChanged();
                }
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    NotifyStateChanged();
                }
            }
        }

        private bool _drawerOpenAppBar = true;
        public bool DrawerOpenAppBar
        {
            get => _drawerOpenAppBar;
            set
            {
                if (_drawerOpenAppBar != value)
                {
                    _drawerOpenAppBar = value;
                    NotifyStateChanged();
                }
            }
        }
    }
}
