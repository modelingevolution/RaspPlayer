using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

using RaspPlayer.RaspberryPis;
using RaspPlayer.Navigation;
using RaspPlayer.UIEvents;

using BlazoReactor.EventAggregator;

namespace RaspPlayer.Preview;

public class VideoPreviewViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private readonly INavigationService _navigation;
    private readonly PubSubEvent<FullscreenOpened> _fullscreenOpened;

    private RaspberryPi _rasp;


    public VideoPreviewViewModel(INavigationService navigation, IMauiEventAggregator mauiEventAggregator)
    {
        _navigation = navigation;
        _fullscreenOpened = mauiEventAggregator.GetEvent<FullscreenOpened>();
    }
    
    public RaspberryPi Rasp
    {
        get => _rasp;
        set
        {
            if (!SetField(ref _rasp, value))
            {
                return;
            }
        }
    }

   

    public ICommand OpenFullscreenCommand => new AsyncRelayCommand(OpenFullscreen);


    
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private async Task OpenFullscreen()
    {
        await _fullscreenOpened.Publish(new FullscreenOpened());
        await _navigation.NavigateToFullscreen(Rasp);
    } 
    
    private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}