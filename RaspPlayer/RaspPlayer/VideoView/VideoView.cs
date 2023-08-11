using RaspPlayer.UIEvents;

namespace RaspPlayer.VideoView;

public class VideoView : View
{
    public static readonly BindableProperty SourceProperty =
        BindableProperty.Create(nameof(Source), typeof(string), typeof(VideoView), "");

    public event EventHandler ShowRequested;
    public event EventHandler HideRequested;
    public event EventHandler RefreshRequested;
    public event EventHandler GoOfflineRequested;
    
    private readonly PubSubEvent<DeviceWentOffline> _deviceWentOffline;
    private readonly PubSubEvent<DeviceWentOnline> _deviceWentOnline;
    private readonly PubSubEvent<StreamStartedOnDevice> _streamStartedOnDevice;
    private readonly PubSubEvent<StreamStoppedOnDevice> _streamStoppedOnDevice;
    private readonly PubSubEvent<StreamStoppedOnAllDevices> _streamStoppedOnAllDevices;
    private readonly PubSubEvent<StreamDisconnected> _streamDisconnected;

    private readonly SubscriptionToken _deviceWentOfflineToken;
    private readonly SubscriptionToken _deviceWentOnlineToken;
    private readonly SubscriptionToken _streamStartedOnDeviceToken;
    private readonly SubscriptionToken _streamStoppedOnDeviceToken;
    private readonly SubscriptionToken _streamStoppedOnAllDevicesToken;
    private readonly SubscriptionToken _streamDisconnectedToken;

    public VideoView()
    {
        var mauiEventAggregator = MauiProgram.Services.GetRequiredService<IMauiEventAggregator>();
        _deviceWentOnline = mauiEventAggregator.GetEvent<DeviceWentOnline>();
        _deviceWentOffline = mauiEventAggregator.GetEvent<DeviceWentOffline>();
        _streamStartedOnDevice = mauiEventAggregator.GetEvent<StreamStartedOnDevice>();
        _streamStoppedOnDevice = mauiEventAggregator.GetEvent<StreamStoppedOnDevice>();
        _streamStoppedOnAllDevices = mauiEventAggregator.GetEvent<StreamStoppedOnAllDevices>();
        _streamDisconnected = mauiEventAggregator.GetEvent<StreamDisconnected>();

        _deviceWentOfflineToken = _deviceWentOffline.Subscribe(OnDeviceWentOffline);
        _deviceWentOnlineToken = _deviceWentOnline.Subscribe(OnDeviceWentOnline);
        _streamStartedOnDeviceToken = _streamStartedOnDevice.Subscribe(OnStreamStartedOnDevice);
        _streamStoppedOnDeviceToken = _streamStoppedOnDevice.Subscribe(OnStreamStoppedOnDevice);
        _streamStoppedOnAllDevicesToken = _streamStoppedOnAllDevices.Subscribe(OnStreamStoppedOnAllDevices);
        _streamDisconnectedToken = _streamDisconnected.Subscribe(OnStreamDisconnected);
    }
    
    public string Source
    {
        get => (string)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public void Show()
    {
        Handler?.Invoke(nameof(ShowRequested));
    }

    public void Hide()
    {
        Handler?.Invoke(nameof(HideRequested));
    }

    public void Refresh()
    {
        Handler?.Invoke(nameof(RefreshRequested));
    }

    public void GoOffline()
    {
        Handler?.Invoke(nameof(GoOfflineRequested));
    }
    
    private async Task OnDeviceWentOnline(DeviceWentOnline @event)
    {
        if (@event.DeviceSerialNumber != Source)
        {
            return;
        }
        
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            IsVisible = true;
            Refresh();
        });
    }

    private async Task OnDeviceWentOffline(DeviceWentOffline @event)
    {
        if (@event.DeviceSerialNumber != Source)
        {
            return;
        }
        
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            IsVisible = false;
        });
    }
    
    private void OnStreamStartedOnDevice(StreamStartedOnDevice @event)
    {
        if (@event.DeviceSerialNumber != Source)
        {
            return;
        }

        Refresh();
    }

    private void OnStreamStoppedOnDevice(StreamStoppedOnDevice @event)
    {
        if (@event.DeviceSerialNumber != Source)
        {
            return;
        }
        
        GoOffline();
    }
    
    private void OnStreamStoppedOnAllDevices(StreamStoppedOnAllDevices @event)
    {
        GoOffline();
    }
    
    private void OnStreamDisconnected(StreamDisconnected @event)
    {
        if (@event.DeviceSerialNumber != Source)
        {
            return;
        }
        
        Refresh();
    }
    
    public void Close()
    {
        _deviceWentOffline.Unsubscribe(_deviceWentOfflineToken);
        _deviceWentOnline.Unsubscribe(_deviceWentOnlineToken);
        _streamStartedOnDevice.Unsubscribe(_streamStartedOnDeviceToken);
        _streamStoppedOnDevice.Unsubscribe(_streamStoppedOnDeviceToken);
        _streamStoppedOnAllDevices.Unsubscribe(_streamStoppedOnAllDevicesToken);
        _streamDisconnected.Unsubscribe(_streamDisconnectedToken);
    }
}