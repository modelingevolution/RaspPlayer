using System.Reflection;
using CommunityToolkit.Maui;

using RaspPlayer.Fullscreen;
using RaspPlayer.RaspberryPis;
using RaspPlayer.Navigation;
using RaspPlayer.Player;
using RaspPlayer.Preview;
using RaspPlayer.UIEvents;
using RaspPlayer.VideoSource;
using RaspPlayer.VideoView;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


using ModelingEvolution.Plumberd.Querying;
using MauiApp = Microsoft.Maui.Hosting.MauiApp;
using CommunityToolkit.Maui.Storage;

namespace RaspPlayer;

public static class MauiProgram
{
    private static readonly ServiceProviderProxy ServiceProviderProxy = new();

    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("MaterialDesignIcons.ttf", "MaterialDesignIcons");
            })
            .UseJsonConfiguration()
            .RegisterModels()
            .RegisterViewModels()
            .RegisterViews()
            .RegisterServices()
            .ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler(typeof(VideoView.VideoView), typeof(VideoViewHandler));
            })
            .AddLogging();

#if DEBUG
        builder.Logging.AddDebug();
#endif
        MauiApp app = builder.Build();
        Services = app.Services;
        ServiceProviderProxy.SetProvider(app.Services);
        
        return app;
    }

    public static IServiceProvider Services { get; private set; }
}

static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder RegisterModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<Players>();

        mauiAppBuilder.Services.AddSingleton<RaspberryPis.RaspberryPis>();
        mauiAppBuilder.Services.AddSingleton<IRaspberryPisLookup>(serviceProvider =>
            serviceProvider.GetRequiredService<RaspberryPis.RaspberryPis>());
        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<AppShell>();
        mauiAppBuilder.Services.AddTransient<MainPage>();
        mauiAppBuilder.Services.AddTransient<VideoPreview>();
        mauiAppBuilder.Services.AddTransient<FullscreenVideo>();
      
        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<VideoPreviewViewModel>();
        mauiAppBuilder.Services.AddTransient<FullscreenVideoViewModel>();

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<MultiplexerProxy>();

        mauiAppBuilder.Services.AddSingleton<VideoPreviewFactory>();
        mauiAppBuilder.Services.AddSingleton<MultiplexerOptions>();

        mauiAppBuilder.Services.AddSingleton<INavigationService, NavigationService>();
        mauiAppBuilder.Services.AddSingleton<IMauiEventAggregator, MauiEventAggregator>();
        mauiAppBuilder.Services.AddSingleton<RaspberryPis.RaspberryPis>();


        return mauiAppBuilder;
    }


    public static MauiAppBuilder UseJsonConfiguration(this MauiAppBuilder mauiAppBuilder)
    {
        var assembly = Assembly.GetExecutingAssembly();

        using Stream mainSettingsStream = assembly.GetManifestResourceStream("RaspPlayer.appsettings.json");

        if (mainSettingsStream is null)
        {
            throw new FileNotFoundException(
                "The configuration file 'appsettings.json' was not found and is not optional.");
        }

        IConfigurationBuilder config = new ConfigurationBuilder()
            .AddJsonStream(mainSettingsStream);

#if DEBUG
        using Stream machineSettingsStream = assembly.GetManifestResourceStream("RaspPlayer.appsettings.Dev.json");
        if (machineSettingsStream is not null)
        {
            config.AddJsonStream(machineSettingsStream);
        }
#endif
        mauiAppBuilder.Configuration.AddConfiguration(config.Build());

        return mauiAppBuilder;
    }

   

    public static MauiAppBuilder AddLogging(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddLogging(options => { options.AddConsole(); });

        return mauiAppBuilder;
    }
}