using Android.OS;

namespace MePlayer.Platforms.Android;

public class MauiVideoViewHandler : Handler
{
    private readonly Action<Message> _messageHandler;

    public MauiVideoViewHandler(Action<Message> messageHandler) : base(Looper.MyLooper())
    {
        _messageHandler = messageHandler;
    }

    public override void HandleMessage(Message message)
    {
        _messageHandler(message);
    }
}