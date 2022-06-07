namespace PrismApp1.CoreApp.Views;

public class MainPage : ContentPage
{
    private readonly DeviceIdiom _idiom;
    private readonly IRegionManager _regionManager;
    public MainPage(IDeviceInfo device, IRegionManager regionManager)
    {
        SetValue(ViewModelLocator.AutowireViewModelProperty, ViewModelLocatorBehavior.Disabled);
        Title = ".NET MAUI - Developers & Influencers";
        _idiom = device.Idiom;
        _regionManager = regionManager;
        ListRegion = new ContentView();
        ListRegion.SetValue(Prism.Regions.Xaml.RegionManager.RegionNameProperty, RegionNames.InfluencerList);
        Content = device.Idiom == DeviceIdiom.TV || device.Idiom == DeviceIdiom.Desktop || device.Idiom == DeviceIdiom.Tablet
            ? CreateDesktopView()
            : CreateMobileView();
    }

    private View ListRegion { get; }

    protected override async void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        if (_idiom != DeviceIdiom.Tablet)
        {
            return;
        }

        if(width > height)
        {
            if (Parent is NavigationPage navPage && navPage.CurrentPage != this)
                await navPage.PopAsync();

            Content = CreateDesktopView();
        }
        else
        {
            Content = CreateMobileView();
        }
    }

    private View CreateDesktopView()
    {
        var grid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star)
            }
        };
        grid.Add(ListRegion);

        var details = new ContentView();
        details.SetValue(Prism.Regions.Xaml.RegionManager.RegionNameProperty, RegionNames.InfluencerDetails);
        grid.Add(details, 1);
        return grid;
    }

    private View CreateMobileView()
    {
        if(_regionManager.Regions.ContainsRegionWithName(RegionNames.InfluencerDetails))
        {
            _regionManager.Regions.Remove(RegionNames.InfluencerDetails);
        }

        return ListRegion;
    }
}
