using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Input;
using RaspPlayer.RaspberryPis;
using RaspPlayer.Navigation;
using Command = Microsoft.Maui.Controls.Command;
using ICommand = System.Windows.Input.ICommand;

namespace RaspPlayer.Fullscreen;

public class FullscreenVideoViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
        
    private readonly INavigationService _navigation;

    private Guid _machineId;
    private string _source;
    private string _name;
    private bool _toolbarVisible = true;

    public FullscreenVideoViewModel(INavigationService navigation)
    {
        _navigation = navigation;
    }
    
    public string Source
    {
        get => _source;
        set => SetField(ref _source, value);
    }

    public string Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }

    public Guid MachineId
    {
        get => _machineId;
        private set => SetField(ref _machineId, value);
    }

    public bool ToolbarVisible
    {
        get => _toolbarVisible;
        set => SetField(ref _toolbarVisible, value);
    }
    
    public void SetRaspberryPi(RaspberryPi machine)
    {
        Source = machine.DeviceSerialNumber;
        Name = machine.Name;
        MachineId = machine.Id;
    }

    public ICommand CloseFullscreenCommand => new AsyncRelayCommand(CloseFullscreen);
    public ICommand ChangeToolbarStateCommand => new Command(ChangeToolbarState);
    
    private async Task CloseFullscreen()
    {
        await _navigation.NavigateBack();
    }

    private void ChangeToolbarState()
    {
        ToolbarVisible = !ToolbarVisible;
    }
    
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}