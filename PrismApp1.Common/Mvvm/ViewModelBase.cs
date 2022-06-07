namespace PrismApp1.Common.Mvvm;

public abstract class ViewModelBase : BindableBase, IPageLifecycleAware, IInitialize, INavigationAware, IDestructible
{
    private readonly BaseServices _baseServices;

    protected ViewModelBase(BaseServices baseServices)
    {
        _baseServices = baseServices;
    }

    protected INavigationService NavigationService => _baseServices.NavigationService;

    protected IRegionManager RegionManager => _baseServices.RegionManager;

    protected IPageDialogService PageDialogs => _baseServices.PageDialogs;

    protected IEventAggregator EventAggregator => _baseServices.EventAggregator;

    private string? _title;
    public string? Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    protected virtual void OnAppearing()
    {
    }

    protected virtual void OnDisappearing()
    {
    }

    protected virtual void Initialize(INavigationParameters parameters)
    {
    }

    protected virtual void OnNavigatedFrom(INavigationParameters parameters)
    {
    }

    protected virtual void OnNavigatedTo(INavigationParameters parameters)
    {
    }

    protected virtual void Destroy()
    {
    }

    void IPageLifecycleAware.OnAppearing()
    {
        OnAppearing();
    }

    void IPageLifecycleAware.OnDisappearing()
    {
        OnDisappearing();
    }

    void IInitialize.Initialize(INavigationParameters parameters)
    {
        if (parameters.TryGetValue<string>("Title", out var title))
            Title = title;

        Initialize(parameters);
    }

    void INavigatedAware.OnNavigatedFrom(INavigationParameters parameters)
    {
        OnNavigatedFrom(parameters);
    }

    void INavigatedAware.OnNavigatedTo(INavigationParameters parameters)
    {
        OnNavigatedTo(parameters);
    }

    void IDestructible.Destroy()
    {
        Destroy();
    }
}
