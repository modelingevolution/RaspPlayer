using RaspPlayer.Platforms.Android;
using Microsoft.Maui.Handlers;

namespace RaspPlayer.VideoView;

public partial class VideoViewHandler : ViewHandler<VideoView, MauiVideoView>
{
    public static readonly IPropertyMapper<VideoView, VideoViewHandler> PropertyMapper =
        new PropertyMapper<VideoView, VideoViewHandler>
        {
            [nameof(VideoView.Source)] = MapSource
        };


    public static readonly CommandMapper<VideoView, VideoViewHandler> CommandMapper = new(ViewCommandMapper)
    {
        [nameof(VideoView.ShowRequested)] = MapShow,
        [nameof(VideoView.HideRequested)] = MapHide,
        [nameof(VideoView.RefreshRequested)] = MapRefresh,
        [nameof(VideoView.GoOfflineRequested)] = MapGoOffline
    };
    
    public VideoViewHandler() : base(PropertyMapper, CommandMapper)
    {
    }
}