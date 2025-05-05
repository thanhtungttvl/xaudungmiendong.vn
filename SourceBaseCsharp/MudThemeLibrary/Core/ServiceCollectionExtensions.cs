using Blazored.LocalStorage;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using MudThemeLibrary.Business.Services;
using MudThemeLibrary.Handlers;

namespace MudThemeLibrary.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMudThemeLibraryServices(this IServiceCollection services)
        {
            services.AddSingleton<IAppConfigService>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                return new AppConfigService(configuration);
            });

            services
                .AddScoped<CryptoInteropHandler>()
                .AddScoped(sp =>
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();

                    return new SecureLocalStorageHandler(
                        sp.GetRequiredService<ILocalStorageService>(),
                        sp.GetRequiredService<CryptoInteropHandler>(),
                        configuration["SecureLocalStorageKey"] ?? "ttvl" // Key mã hóa
                    );
                });

            services.AddBlazoredLocalStorage();
            services.AddSweetAlert2();
            services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
                config.SnackbarConfiguration.PreventDuplicates = false;
                config.SnackbarConfiguration.VisibleStateDuration = 5000;
                config.SnackbarConfiguration.MaxDisplayedSnackbars = int.MaxValue;
            });
            services.AddMudMarkdownServices();

            return services;
        }
    }
}
