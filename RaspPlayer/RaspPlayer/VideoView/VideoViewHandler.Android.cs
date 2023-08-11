using MePlayer.Platforms.Android;

namespace MePlayer.VideoView;

public partial class VideoViewHandler
{

    protected override MauiVideoView CreatePlatformView() => new MauiVideoView(Context, VirtualView);

    public static void MapSource(VideoViewHandler handler, VideoView videoView)
    {
        handler.PlatformView.UpdateSource();
    }

    public static void MapShow(VideoViewHandler handler, VideoView videoView, object args)
    {
        handler.PlatformView.Show();
    }
    
    public static void MapHide(VideoViewHandler handler, VideoView videoView, object args)
    {
        handler.PlatformView.Hide();
    }
    
    public static void MapRefresh(VideoViewHandler handler, VideoView videoView, object args)
    {
        handler.PlatformView.Refresh();
    }
    
    public static void MapGoOffline(VideoViewHandler handler, VideoView videoView, object args)
    {
        handler.PlatformView.GoOffline();
    }
    
    protected override void DisconnectHandler(MauiVideoView platformView)
    {
        platformView.Dispose();
        base.DisconnectHandler(platformView);
    }
}