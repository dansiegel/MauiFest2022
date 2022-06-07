using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrismApp1.Authentication.ViewModels;
using PrismApp1.Common.Mvvm;
using PrismApp1.CoreApp;

namespace PrismApp1.ViewModels;

internal class SplashPageViewModel : ViewModelBase
{
    private IModuleManager _moduleManager { get; }

    public SplashPageViewModel(BaseServices baseServices, IModuleManager moduleManager)
        : base(baseServices)
    {
        _moduleManager = moduleManager;
    }

    protected override void OnNavigatedTo(INavigationParameters parameters)
    {
        if (parameters.TryGetValue<bool>("authenticated", out var authenticated) && authenticated)
        {
            if (!_moduleManager.IsModuleInitialized("CoreAppModule"))
                _moduleManager.LoadModule("CoreAppModule");

            NavigationService.CreateBuilder()
                .UseAbsoluteNavigation()
                .AddNavigationPage()
                .AddNavigationSegment("MainPage")
                .Navigate();
        }
        else
        {
            NavigationService.CreateBuilder()
                .UseAbsoluteNavigation()
                .AddNavigationSegment<LoginPageViewModel>()
                .Navigate();
        }
    }
}
