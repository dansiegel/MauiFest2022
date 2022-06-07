using PrismApp1.Authentication.ViewModels;
using PrismApp1.Authentication.Views;

namespace PrismApp1.Authentication;

public class AuthenticationModule : IModule
{
    public void OnInitialized(IContainerProvider containerProvider)
    {

    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
    }
}