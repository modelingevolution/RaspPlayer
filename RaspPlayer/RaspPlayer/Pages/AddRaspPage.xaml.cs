using System.Collections.Specialized;
using BlazoReactor.EventAggregator;
using System.Reflection.PortableExecutable;
using System.Text;
using CommunityToolkit.Maui.Storage;
using RaspPlayer.RaspberryPis;
using RaspPlayer.UIEvents;
using RaspPlayer.VideoSource;

using Microsoft.Extensions.Logging;
using RaspPlayer.Device;
using RaspPlayer.Preview;
using ContentPage = Microsoft.Maui.Controls.ContentPage;
using System.Text.Json;

namespace RaspPlayer;

public partial class AddRaspPage : ContentPage
{
    private readonly RaspberryPis.RaspberryPis _rasps;
    private readonly ILogger<MainPage> _logger;


    public AddRaspPage(RaspberryPis.RaspberryPis rasps)
    {
        _rasps = rasps;

        InitializeComponent();
        

    }

    public async Task SaveCamerasConfig()
    {
        string path = Path.Combine(FileSystem.AppDataDirectory, "camerasConfig.json");
        var jsonString = JsonSerializer.Serialize(_rasps.Index);
        await File.WriteAllTextAsync(path, jsonString);
        
    }
    async void OnAddRasp(object sender, EventArgs args)
    {
       _rasps.Index.Add(new RaspberryPi(Guid.NewGuid(),Name.Text,Hostname.Text ));
       await SaveCamerasConfig();
    }
    protected override bool OnBackButtonPressed()
    {
         Navigation.PopAsync();
        return base.OnBackButtonPressed();
    }
}