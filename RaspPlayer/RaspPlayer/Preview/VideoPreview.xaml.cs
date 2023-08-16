using RaspPlayer.RaspberryPis;
using RaspPlayer.Player;

namespace RaspPlayer.Preview;

public partial class VideoPreview : ContentView
{
    private readonly VideoPreviewViewModel _viewModel;
    
    public VideoPreview(VideoPreviewViewModel viewModel)
    {
        _viewModel = viewModel;
        InitializeComponent();
    }

    public VideoPreviewViewModel ViewModel => _viewModel;
    
    public void SetRasp(RaspberryPi rasp)
    {
        _viewModel.Rasp = rasp;
    }
    
    public void Show()
    {
        Video.Show();
    }

    public void Hide()
    {
        Video.Hide();
    }
}