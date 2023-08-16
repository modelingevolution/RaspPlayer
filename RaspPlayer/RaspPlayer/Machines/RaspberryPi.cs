using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RaspPlayer.RaspberryPis;

public class RaspberryPi : INotifyPropertyChanged
{
    private Guid _id;
    private string _name;
    private string _deviceSerialNumber;
    
    public RaspberryPi(Guid id, string name, string deviceSerialNumber)
    {
        Id = id;
        Name = name;
        DeviceSerialNumber = deviceSerialNumber;
    }

    public Guid Id
    {
        get => _id;
        private init => SetField(ref _id, value);
    }

    public string Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }

    public string DeviceSerialNumber
    {
        get => _deviceSerialNumber;
        set => SetField(ref _deviceSerialNumber, value);
    }

    public event PropertyChangedEventHandler PropertyChanged;

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
};