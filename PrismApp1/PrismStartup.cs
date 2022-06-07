using Microsoft.Extensions.Logging;
using PrismApp1.Authentication;
using PrismApp1.Common.Mvvm;
using PrismApp1.CoreApp;
using PrismApp1.ViewModels;
using PrismApp1.Views;

namespace PrismApp1;

internal static class PrismStartup
{
    public static void Configure(PrismAppBuilder builder)
    {
        builder.RegisterTypes(RegisterTypes)
            .ConfigureLogging(logging => logging.AddConsole())
            .ConfigureModuleCatalog(ConfigureModuleCatalog)
            .OnInitialized(OnInitialized)
            .OnAppStart("LoginPage");
    }

    private static void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<SplashPage, SplashPageViewModel>("Splash")
            .RegisterScoped<BaseServices>()
            .RegisterInstance(SemanticScreenReader.Default)
            .RegisterInstance(DeviceInfo.Current)
            .RegisterInstance(Launcher.Default);
    }

    private static void OnInitialized(IContainerProvider container)
    {
        
    }

    private static void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
        moduleCatalog.AddModule<AuthenticationModule>()
            .AddModule<CoreAppModule>(InitializationMode.OnDemand);
    }
}
