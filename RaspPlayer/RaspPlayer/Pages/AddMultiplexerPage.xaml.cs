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
using System.Text.Json;

namespace RaspPlayer;

public partial class AddMultiplexerPage : ContentPage
{
    public MultiplexerOptions MultiplexerOptions { get; set; }
    private readonly ILogger<MainPage> _logger;

    public AddMultiplexerPage(MultiplexerOptions mOptions)
    {
        MultiplexerOptions = mOptions;
        
       
        InitializeComponent();
    }
    void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
     
    }
    public async Task SaveMultiplexerConfig()
    {
        string path = Path.Combine(FileSystem.AppDataDirectory, "multiplexerConfig.json");
        var jsonString = JsonSerializer.Serialize(MultiplexerOptions);
        await File.WriteAllTextAsync(path, jsonString);

    }
    async void OnSaveOptions(object sender, EventArgs args)
    {
        MultiplexerOptions.Host = Hostname.Text;
        MultiplexerOptions.Port = Int32.Parse(Port.Text);
        await SaveMultiplexerConfig();
    }
    protected override bool OnBackButtonPressed()
    {
         Navigation.PopAsync();
        return base.OnBackButtonPressed();
    }
}