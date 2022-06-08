# Prism Demo - MauiFest 2022

This is the Demo App for Prism.Maui from [MauiFest 2022](https://youtube.com/watch?v=UOBOsBbmmTw).

<img src="resources/prism-loves-maui.png" style="max-width:500px;display:block;margin-left:auto;margin-right:auto;" />

## Support

The Prism Library is open source and is available for free under the MIT license. Please consider supporting the project by becoming a [GitHub Sponsor](https://xam.dev/sponsorDan).

Enterprise Support for Prism is additionally available through [AvantiPoint](https://avantipoint.com), with plans that can be tailored for those who may need several hours of support per month, or for those who just need less frequent help throughout the year.

## Running the Project

Please note this demo is currently using a CI build of Prism.Maui which is only available for GitHub Sponsors via the [SponsorConnect NuGet Feed](https://sponsorconnect.dev). Some APIs shown in this Demo are not yet publicly available, and will be in the next public release.

## Key Concepts

- Using the PrismAppBuilder to register your services
- Using Prism Modules to break your app into smaller parts and delay unnecessary service registration until you need them
- Building Dynamic User Experiences with Prism Regions
- Using the NavigationBuilder API to Navigate
- Using the new NavigationBuilder to create your Navigation URI or use ViewModel based Navigation.

### Additional Concepts

- Providing a Delegate for the DelegateCommand.CanExecute
- Observing Property Changes to re-evaluate DelegateCommand.CanExecute
- Disabling the ViewModelLocator
- Using a Mix of XAML & Code for your Pages
- Using Maui Essentials Interfaces within ViewModels to maintain testability
- Adding a "BaseServices" class as a Single parameter within a Base ViewModel to better allow changes as your app grows

## Service Registration

This demo & the Prism.Maui template add a PrismStartup file. This is not actually necessary, but is a good example of how to keep potentially extensive code out of the MauiAppBuilder workflow keeping it easier to maintain and follow what you're adding to the app. In the PrismStartup file, we register the Services and Prism Modules. The Services are registered with Prism's own Ioc abstraction interface `IContainerRegistry`.

```cs
private static void RegisterTypes(IContainerRegistry containerRegistry)
{
    containerRegistry.RegisterForNavigation<SplashPage, SplashPageViewModel>("Splash")
        .RegisterScoped<BaseServices>()
        .RegisterInstance(SemanticScreenReader.Default)
        .RegisterInstance(DeviceInfo.Current)
        .RegisterInstance(Launcher.Default);
}
```

In addition to supporting Prism's built in Ioc abstraction, Prism.Maui additionally fully supports the Microsoft.Extensions.DependencyInjection `IServiceCollection`. This means that you can use either, or both, to register your services as needed. The Platform Specific extensions for Registering Views for Page Navigation, Region Navigation or Dialogs are always duplicated across to ensure you can register what you need to where you need to.

```cs
private static void ConfigureServices(IServiceCollection services)
{
    services.RegisterForNavigation<SplashPage, SplashPageViewModel>("Splash")
        .AddScoped<BaseServices>()
        .AddSingleton(SemanticScreenReader.Default)
        .AddSingleton(DeviceInfo.Current)
        .AddSingleton(Launcher.Default);
}
```

### Built In Registrations

By Default Prism.Maui will automatically register the MAUI TabbedPage & NavigationPage if you have not registered them yourself. This reduces the need for you to explicitly register these pages, unless you have a custom implementation that you absolutely need to use.

#### Flyout Page

While this app does not make use of the FlyoutPage (formerly the MasterDetailPage), it is worth noting that when using the FlyoutPage you should NOT specify a Detail page. You should add a Flyout. The Flyout will make use of the FlyoutPage's ViewModel & NavigationService. When using a FlyoutPage you can set the Navigation Stack properly with a uri like `MyFlyoutPage/NavigationPage/MyPage`.

#### Navigation Page

Prism.Maui improves on the behavior we had in Prism.Forms by introducing a PrismNavigationPage. It is recommended that if you absolutely need to create a custom NavigationPage, you should either inherit from this as your base class, or look at the source code to be able to update your custom NavigationPage with the necessary code to properly utilize Prism's NavigationService to handle when the user taps the back button. This is automatically registered with the navigation key "NavigationPage". Remember that you can globally style your NavigationPage using an implicit style. This is already done for you when creating a new MAUI project or using the Prism.Maui template.

#### Tabbed Page

Going back to Prism 7 (I think)... Prism has supported dynamically building Tabbed Pages using the URI. You can do this with a URI like `TabbedPage?createTab=ViewA&createTab=ViewB&createTab=ViewC`. This is a great way to create a Tabbed Page without having to create a custom TabbedPage class. As of this Demo the support for TabbedPages is identical to Prism.Forms. The Prism 9.0 release will include additional support for dynamically building TabbedPages with deep linking for each `createTab` parameter. Each value will have to be properly URI Encoded. For these complex navigation patterns you may find it easier to use the NavigationBuilder which can allow you to more easily create these deep links for each tab.

## Architecture

This app is architected to take advantage of Prism Modularity. While this is absolutely a very simple Demo App, it is a good example of how to use the Prism Modularity API to break your app into smaller parts and delay unnecessary service registration until you need them. In large production applications this can have a significant impact on the startup performance of your app. The pattern is simple. First you will notice in the MAUI single project, it contains a single "Splash Page". This differs from the native Splash Screen as you are working within the context of your MVVM application with full access to the DI container. Here you might perform tasks like checking if it is the application first run, or if you need to perform any data migrations, or if you need to authenticate your user, or what roles/permissions your user has, etc. It is important to note that the "Authentication" in this app is purely for demonstration and is not actually hooked up to anything nor do we actually make use of a store to watch the user's authentication state.

During the bootstrapping of our app in the PrismStartup file, we register some basic services from MAUI Essentials, we register the Splash Page, and we add the Prism Modules. The AuthenticationModule doesn't specify any parameters thus taking the default to initialize `WhenAvailable`. This results in the AuthenticationModule being initialized as soon as the MauiAppBuilder is Built.

The CoreAppModule however is added to the ModuleCatalog as `OnDemand`. This means that it will not be initialized until you manually load the module. You can an example of this in the SplashPageViewModel where we wait to load the CoreAppModule until we are sure we have "authenticated" the user.

### App UI

This sample takes an unconventional approach (on purpose), to show you that while many times MVVM is the best approach to building your app, it does not always make sense for every Page/View. The App is designed to display a List of MAUI Developers & Influencer's from Microsoft & the Community. When you select a developer you will see some details about them. As the name suggests (Multi-platform App UI), we are building an app which may be displayed across multiple platforms & device idioms. To provide a better developer experience we make use of Regions which allow us to develop Views following an MVVM pattern, while we can create pages that contain the business logic to determine how our UI should actually look. As a result, when using the app on a mobile device we will see a MainPage which simply contains our list of Developers & Influencer's. When we select a developer the app will use classic Page based Navigation to Navigate to a Page which simply contains a single Region with a Default View for our InfluencerDetails. This is automatically handled by Prism's RegionManager and the ViewModel is able to make use of Page based Navigation interfaces initializing with Prism's `IInitialize`.

However when we run the app for Windows thus giving us a Desktop environment, we have more screen real estate and instead we have a MainPage which sets up a Grid. This time we get 2 regions, again we get one for the List, and this time we get a second region to hold the details for any developer that we select.

Some additional considerations of note:

- You will notice that in the CoreAppModule, we inject Prism's `IRegionManager`, and Register the Default View with the Region. This automatically ensures that the Region will get the specified view when it is created rather than needing to specify the Attached Property like we see in the MauiDevDetailPage.
- We do not need to specify the `MauiDevDetail` View as a Default when using it on desktop since we instead use Region Navigation to navigate when we have a new selection.
- There is always more than one way to configure this. Based on the implementation that we have, if we had some sort of Tab View we might see a new tab created for each developer that is selected. This is because our implementation of `IsNavigationTarget` in the MauiDevDetailViewModel returns true when it has been initialized for the specified developer. This keeps the View in memory and ready to be navigated back to with the state it had previously. Given the simplicity of the example we could just as easily always return true and simply update the Influencer property each time we navigate.
