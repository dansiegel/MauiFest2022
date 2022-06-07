using PrismApp1.CoreApp.Services;
using PrismApp1.CoreApp.ViewModels;
using PrismApp1.CoreApp.Views;
using Refit;

namespace PrismApp1.CoreApp;

public class CoreAppModule : IModule
{
    public CoreAppModule(IRegionManager regionManager)
    {
        regionManager.RegisterViewWithRegion(RegionNames.InfluencerList, "MauiInfluencers");
    }

    public void OnInitialized(IContainerProvider containerProvider)
    {
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterInstance(new HttpClient
        {
            BaseAddress = new Uri("https://dansiegel.blob.core.windows.net")
        });
        containerRegistry.Register<IMauiDevClient>(c =>
            RestService.For<IMauiDevClient>(c.Resolve<HttpClient>()));

        containerRegistry.RegisterForNavigation<MainPage>()
            .RegisterForNavigation<MauiDevDetailPage>("MauiDevDetail");
        containerRegistry.RegisterForRegionNavigation<MauiDevDetail, MauiDevDetailViewModel>()
            .RegisterForRegionNavigation<MauiInfluencers>();
    }
}
