using PrismApp1.Common.Mvvm;

namespace PrismApp1.Authentication.ViewModels;

public class LoginPageViewModel : ViewModelBase
{
    public LoginPageViewModel(BaseServices baseServices) 
        : base(baseServices)
    {
        LoginCommand = new DelegateCommand<string>(OnLoginCommandExecuted, s => !IsBusy)
            .ObservesProperty(() => IsBusy);
    }

    public DelegateCommand<string> LoginCommand { get; }

    private async void OnLoginCommandExecuted(string provider)
    {
        IsBusy = true;

        try
        {
            // Simulate Authentication Call
            await Task.Delay(250);
            await NavigationService.CreateBuilder()
                .UseAbsoluteNavigation()
                .AddSegment("Splash")
                .AddParameter("authenticated", true)
                .NavigateAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }
}
