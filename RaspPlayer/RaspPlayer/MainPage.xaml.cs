using System.Collections.ObjectModel;
using System.Collections.Specialized;
using BlazoReactor.EventAggregator;
using System.Reflection.PortableExecutable;
using RaspPlayer.RaspberryPis;
using RaspPlayer.UIEvents;
using RaspPlayer.VideoSource;

using Microsoft.Extensions.Logging;
using RaspPlayer.Device;
using RaspPlayer.Preview;
using ContentPage = Microsoft.Maui.Controls.ContentPage;
using static Microsoft.Maui.ApplicationModel.Permissions;
using System.Text.Json;
using CommunityToolkit.Maui.Storage;

namespace RaspPlayer;

public partial class MainPage : ContentPage
{
    private readonly RaspberryPis.RaspberryPis _rasps;
    private readonly MultiplexerProxy _multiplexerProxy;
    private readonly VideoPreviewFactory _videoPreviewFactory;
    private readonly ILogger<MainPage> _logger;
    private readonly MultiplexerOptions _options;


    public MainPage(RaspberryPis.RaspberryPis rasps, MultiplexerProxy multiplexerProxy,MultiplexerOptions options, IMauiEventAggregator eventAggregator, VideoPreviewFactory videoPreviewFactory, ILogger<MainPage> logger)
    {
        _rasps = rasps;
        _multiplexerProxy = multiplexerProxy;
        _videoPreviewFactory = videoPreviewFactory;
        _logger = logger;
        _options = options;
      
        InitializeComponent();

        DeviceDisplay.KeepScreenOn = true;

        _rasps.Index.CollectionChanged += IndexOnCollectionChanged;

        PubSubEvent<FullscreenOpened> fullscreenOpened = eventAggregator.GetEvent<FullscreenOpened>();
        fullscreenOpened.Subscribe(_ => HideAll());

        Task.Run(LoadConfigs);
       
    }

    public async Task LoadConfigs()
    {
        string multiplexerPath = Path.Combine(FileSystem.AppDataDirectory, "multiplexerConfig.json");

        if (File.Exists(multiplexerPath))
        {
            MultiplexerOptions options = JsonSerializer.Deserialize<MultiplexerOptions>(File.ReadAllText(multiplexerPath));
            _options.Host = options.Host;
            _options.Port = options.Port;
        }


        string cameraConfigPath = Path.Combine(FileSystem.AppDataDirectory, "camerasConfig.json");
        if (File.Exists(cameraConfigPath))
        {

            ObservableCollection<RaspberryPi> index = JsonSerializer.Deserialize<ObservableCollection<RaspberryPi>>(File.ReadAllText(cameraConfigPath));
            foreach (var rasp in index)
            {
                _rasps.Index.Add(rasp);
            }
        }
        await Task.Run(ConnectToMultiplexerAndLoadPreviews);


    }

    private void IndexOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        MainThread.InvokeOnMainThreadAsync(() =>
        {
            LoadPreviews();
            LoadingLayout.IsVisible = false;
            PreviewsLayout.IsVisible = true;
        });

    }

    
    private async Task ConnectToMultiplexerAndLoadPreviews()
    {
        while (true)
        {
            bool isOnline = _multiplexerProxy.IsOnline();
            _logger.LogDebug("Is online: {IsOnline}", isOnline);
            if (isOnline)
            {
                _logger.LogDebug("Rasps: {Machines}", _rasps.Index.Count);
                if (!_rasps.Index.Any())
                {
                    _logger.LogDebug("No rasps found");
                    continue;
                }

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    LoadPreviews();
                    LoadingLayout.IsVisible = false;
                    PreviewsLayout.IsVisible = true;
                });

                return;
            }

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                LoadingLabel.Text = "Previews' source is not responding at the moment...";
            });

            await Task.Delay(5_000);

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                LoadingLabel.Text = "Previews are loading...";
            });
        }
    }

    private void LoadPreviews()
    {
        var column = 0;
        var row = 0;
        const int machinesInRow = 3;
        const int visibleRows = 2;
        int numberOfMachines = _rasps.Index.Count;
        int numberOfRows = Math.Max(numberOfMachines / machinesInRow, 3);

        PreviewsGrid.Clear();
        PreviewsGrid.ColumnDefinitions = new ColumnDefinitionCollection();
        PreviewsGrid.RowDefinitions = new RowDefinitionCollection();
        PreviewsGrid.ColumnSpacing = PreviewsGrid.RowSpacing = 2;

        for (var i = 0; i < machinesInRow; i++)
        {
            PreviewsGrid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(machinesInRow, GridUnitType.Star)
            });
        }

        for (var i = 0; i < numberOfRows; i++)
        {
            PreviewsGrid.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(DisplaySize.HeightDpi / visibleRows, GridUnitType.Absolute)
            });
        }


        foreach (RaspberryPi rasp in _rasps.Index)
        {
            VideoPreview videoPreview = _videoPreviewFactory.ForMachine(rasp);

            PreviewsGrid.Add(videoPreview);
            Grid.SetColumn(videoPreview, column);
            Grid.SetRow(videoPreview, row);

            column++;

            if (column < machinesInRow) continue;

            column = 0;
            row++;
        }
    }

    async void OnConfigureMultiplexerButtonClicked(object sender, EventArgs args)
    {
        await Navigation.PushAsync(new AddMultiplexerPage(_options));
    }

    async void OnConnectMultiplexer(object sender, EventArgs args)
    {
        await ConnectToMultiplexerAndLoadPreviews();
    }
    async void OnAddRaspButtonClicked(object sender, EventArgs args)
    {
        await Navigation.PushAsync(new AddRaspPage(_rasps));
    }
    private void HideAll()
    {
        IEnumerable<VideoPreview> enumerable = PreviewsGrid.Children.OfType<VideoPreview>();

        foreach (VideoPreview view in enumerable)
        {
            view.Hide();
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        IEnumerable<VideoPreview> enumerable = PreviewsGrid.Children.OfType<VideoPreview>();

        foreach (VideoPreview view in enumerable)
        {
            view.Show();
        }
    }
}