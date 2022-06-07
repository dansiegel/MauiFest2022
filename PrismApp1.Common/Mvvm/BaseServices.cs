namespace PrismApp1.Common.Mvvm;

public class BaseServices
{
    public BaseServices(
        INavigationService navigationService,
        IRegionManager regionManager,
        IPageDialogService pageDialogService,
        IEventAggregator eventAggregator)
    {
        NavigationService = navigationService;
        RegionManager = regionManager;
        PageDialogs = pageDialogService;
        EventAggregator = eventAggregator;
    }

    public INavigationService NavigationService { get; }

    public IRegionManager RegionManager { get; }

    public IPageDialogService PageDialogs { get; }

    public IEventAggregator EventAggregator { get; }
}