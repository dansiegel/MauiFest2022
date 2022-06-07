using PrismApp1.Common.Mvvm;
using PrismApp1.CoreApp.Models;

namespace PrismApp1.CoreApp.ViewModels;

internal class MauiDevDetailViewModel : ViewModelBase, IRegionAware
{
    private readonly ILauncher _launcher;
    public MauiDevDetailViewModel(BaseServices baseServices, ILauncher launcher)
        : base(baseServices)
    {
        _launcher = launcher;
        OpenGitHubCommand = new(OnOpenGitHubCommandExecuted);
        OpenTwitterCommand = new(OnOpenTwitterCommandExecuted);
    }

    private MauiInfluencer? _influencer;
    public MauiInfluencer? Influencer
    {
        get => _influencer;
        set => SetProperty(ref _influencer, value);
    }

    public DelegateCommand OpenGitHubCommand { get; }

    public DelegateCommand OpenTwitterCommand { get; }

    protected override void Initialize(INavigationParameters parameters)
    {
        if (parameters.TryGetValue<MauiInfluencer>("influencer", out var influencer))
            Influencer = influencer;
    }

    public bool IsNavigationTarget(INavigationContext navigationContext)
    {
        if (Influencer is null)
            return true;
        else if (navigationContext.Parameters.TryGetValue<MauiInfluencer>("influencer", out var influencer))
            return influencer.Name == Influencer.Name;
        return false;
    }

    public void OnNavigatedFrom(INavigationContext navigationContext)
    {

    }

    public void OnNavigatedTo(INavigationContext navigationContext)
    {
        if (Influencer is null && navigationContext.Parameters.TryGetValue<MauiInfluencer>("influencer", out var influencer))
            Influencer = influencer;
    }

    private async void OnOpenTwitterCommandExecuted()
    {
        if (!await _launcher.TryOpenAsync($"https://twitter.com/{Influencer!.Twitter}"))
            await PageDialogs.DisplayAlertAsync("Whoops", $"Unable to open the Twitter page for {Influencer.Name}. Please open https://twitter.com/{Influencer.Twitter}", "Ok");
    }

    private async void OnOpenGitHubCommandExecuted()
    {
        if (!await _launcher.TryOpenAsync($"https://github.com/{Influencer!.GitHub}"))
            await PageDialogs.DisplayAlertAsync("Whoops", $"Unable to open the GitHub page for {Influencer.Name}. Please open https://github.com/{Influencer.GitHub}", "Ok");
    }
}
