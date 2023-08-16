using RaspPlayer.RaspberryPis;
using RaspPlayer.UIEvents;
using BlazoReactor.EventAggregator;

namespace RaspPlayer.Fullscreen;

public partial class FullscreenVideo : ContentPage, IDisposable
{
    private readonly FullscreenVideoViewModel _viewModel;
    private readonly PubSubEvent<CurrentRaspChanged> _currentMachineChanged;

    public FullscreenVideo(FullscreenVideoViewModel viewModel, IMauiEventAggregator eventAggregator)
    {
        _viewModel = viewModel;
        BindingContext = viewModel;

        _currentMachineChanged = eventAggregator.GetEvent<CurrentRaspChanged>();

        InitializeComponent();
        
        DeviceDisplay.KeepScreenOn = true;
    }

    public async Task SetMachine(RaspberryPi rasp)
    {
        var currentMachineChanged = new CurrentRaspChanged
        {
            RaspId = rasp.Id
        };
        await _currentMachineChanged.Publish(currentMachineChanged);
        _viewModel.SetRaspberryPi(rasp);
     
    }

    public void Dispose()
    {
        Video?.Close();
    }

    private void OnReturnButtonClicked(object sender, EventArgs e)
    {
        Dispose();
    }
}