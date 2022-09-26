using PrismApp1.Common.Mvvm;
using PrismApp1.CoreApp.Collections;
using PrismApp1.CoreApp.Models;
using PrismApp1.CoreApp.Services;

namespace PrismApp1.CoreApp.ViewModels;

public class MauiInfluencersViewModel : ViewModelBase
{
    private readonly IMauiDevClient _client;
    public MauiInfluencersViewModel(BaseServices baseServices, IMauiDevClient client)
        : base(baseServices)
    {
        _client = client;
        Influencers = new();
        InfluencerSelectedCommand = new (OnInfluencerSelectedCommandExecuted);
    }

    public ObservableRangeCollection<MauiInfluencer> Influencers { get; }

    public DelegateCommand<MauiInfluencer> InfluencerSelectedCommand { get; }

    protected override async void OnNavigatedTo(INavigationParameters parameters)
    {
        if (parameters.GetNavigationMode() == Prism.Navigation.NavigationMode.Back)
            return;

        using var response = await _client.GetInfluencers();
        if (!response.IsSuccessStatusCode)
            await PageDialogs.DisplayAlertAsync("Whoops", "Something went wrong, we weren't able to load the list of Maui Influencers", "Ok");
        else
            Influencers.ReplaceRange(response.Content.OrderBy(_ => new Random().Next()));
    }

    private void OnInfluencerSelectedCommandExecuted(MauiInfluencer influencer)
    {
        if(RegionManager.Regions.ContainsRegionWithName(RegionNames.InfluencerDetails))
        {
            RegionManager.RequestNavigate(RegionNames.InfluencerDetails, "MauiDevDetail", new NavigationParameters
            {
                { "influencer", influencer }
            });
            return;
        }

        NavigationService.CreateBuilder()
            .AddSegment("MauiDevDetail")
            .AddParameter("influencer", influencer)
            .Navigate();
    }
}
