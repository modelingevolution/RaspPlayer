using System.Timers;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using MePlayer.Player;
using MePlayer.VideoView;
using Microsoft.Maui.Controls.Compatibility;
using RaspPlayer;
using RaspPlayer.Player;
using Color = Android.Graphics.Color;
using ProgressBar = Android.Widget.ProgressBar;
using Timer = System.Timers.Timer;

namespace MePlayer.Platforms.Android;

public class MauiVideoView : RelativeLayout, IMauiVideoView, ISurfaceHolderCallback
{
    private readonly Context _context;
    private readonly VideoView.VideoView _videoView;
    private readonly Handler _handler;
    private readonly Players _players;
    private NativePlayer _player;
    private SurfaceView _video;
    private TextView _videoOfflineText;
    private ProgressBar _loader;
    private Surface _videoSurface;
    private bool _visible;

    public MauiVideoView(Context context, VideoView.VideoView videoView) : base(context)
    {
        _context = context;
        _videoView = videoView;

        _handler = new MauiVideoViewHandler(HandlerAction);
        _visible = true;
        
        SetBackgroundColor(Color.Black);

        _videoOfflineText = new TextView(context);
        _videoOfflineText.Text = "Preview is offline";
        var textLayoutParams = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
        _videoOfflineText.LayoutParameters = textLayoutParams;
        _videoOfflineText.Gravity = GravityFlags.Center;
        _videoOfflineText.SetTextColor(Color.ParseColor("#666666"));
        _videoOfflineText.Visibility = ViewStates.Invisible;
        

        if (string.IsNullOrEmpty(videoView.Source))
        {
            AddView(_videoOfflineText);
            _videoOfflineText.Visibility = ViewStates.Visible;
            return;
        }

        _video = new SurfaceView(context);
        var videoLayoutParams = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
        videoLayoutParams.AddRule(LayoutRules.CenterInParent);
        _video.LayoutParameters = videoLayoutParams;
        _video.Holder?.AddCallback(this);

        _loader = new ProgressBar(context);
        var loaderLayoutParams = new LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
        loaderLayoutParams.AddRule(LayoutRules.CenterInParent);
        loaderLayoutParams.SetMargins(20.DpToPx(), 20.DpToPx(), 20.DpToPx(), 20.DpToPx());
        _loader.LayoutParameters = loaderLayoutParams;
        
        AddView(_video);
        AddView(_loader);
        AddView(_videoOfflineText);

        _players = MauiProgram.Services.GetRequiredService<Players>();
        _player = _players.GetOrAdd(videoView.Source);
        
        _player.SetPlayerMessageHandler(_handler);
        Play();

        if (_player.IsVideoOffline)
        {
            _loader.Visibility = ViewStates.Gone;
            _videoOfflineText.Visibility = ViewStates.Visible;
        }
    }
    
    private void HandlerAction(Message message)
    {
        switch (message.What)
        {
            case PlayerMessages.UpdateProgress:
                if (_loader?.Visibility == ViewStates.Gone) return;
                _player.IsVideoOffline = false;
                if (_handler is null) return;
                _handler.SendEmptyMessageDelayed(PlayerMessages.UpdateProgress, 200);
                long progress = _player?.GetParam(PlayerParameters.MediaPosition) ?? 0;
                if (_loader is null) return;
                _loader.Visibility = progress == 0 ? ViewStates.Visible : ViewStates.Invisible;
                break;

            case PlayerMessages.UpdateViewSize:
                if (_video is null) return;
                _video.Visibility = ViewStates.Visible;
                break;

            case PlayerMessages.OpenDone:
                _player?.SetDisplaySurface(_videoSurface);
                if (_video is null) return;
                _video.Visibility = ViewStates.Invisible;
                if (_handler is null) return;
                _handler.SendEmptyMessage(PlayerMessages.UpdateViewSize);
                Play();
                break;

            case PlayerMessages.OpenFailed:
                _player.IsVideoOffline = true;
                if (_videoOfflineText is null) return;
                _videoOfflineText.Visibility = ViewStates.Visible;
                if (_loader is null) return;
                _loader.Visibility = ViewStates.Gone;
                break;

            case PlayerMessages.VideoResized:
                if (_video is null) return;
                _video.Visibility = ViewStates.Invisible;
                if (_handler is null) return;
                _handler.SendEmptyMessage(PlayerMessages.UpdateViewSize);
                break;
        }
    }

    #region Properties update

    public void UpdateSource()
    {
    }

    #endregion

    #region Commands

    public void Show()
    {
        if (_video is null) return;
        
        _visible = true;
        _video.Visibility = ViewStates.Visible;
        _player?.SetDisplaySurface(_videoSurface);
        if (!(_player?.IsVideoOffline ?? false))
        {
            _videoOfflineText.Visibility = ViewStates.Invisible;
        }
    }

    public void Hide()
    {
        if (_video is null) return;
        
        _visible = false;
        _video.Visibility = ViewStates.Invisible;
        _player?.SetDisplaySurface(null);
    }

    public void Refresh()
    {
        if (!_visible)
        {
            return;
        }

        _player?.SetDisplaySurface(null);

        _player = _players.GetReopenedPlayer(_videoView.Source);
        _player?.SetPlayerMessageHandler(_handler);
        _player?.SetDisplaySurface(_videoSurface);

        MainThread.InvokeOnMainThreadAsync(() =>
        {
            _loader.Visibility = ViewStates.Visible;
            _videoOfflineText.Visibility = ViewStates.Invisible;
        }).GetAwaiter().GetResult();

        Play();
    }

    public void GoOffline()
    {
        if (_video is null) return;
        
        MainThread.InvokeOnMainThreadAsync(() =>
        {
            _player.Close();
            _player.IsVideoOffline = true;
            _loader.Visibility = ViewStates.Gone;
            _videoOfflineText.Visibility = ViewStates.Visible;
            _video.Visibility = ViewStates.Invisible;
        }).GetAwaiter().GetResult();
    }

    #endregion

    #region SurfaceHolder Callbacks

    public void SurfaceChanged(ISurfaceHolder holder, Format format, int width, int height)
    {
        _player.SetDisplaySurface(holder.Surface);
    }

    public void SurfaceCreated(ISurfaceHolder holder)
    {
        _videoSurface = holder.Surface;
        _player.SetDisplaySurface(holder.Surface);
    }

    public void SurfaceDestroyed(ISurfaceHolder holder)
    {
        _player.SetDisplaySurface(null);
    }

    #endregion

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _video = null;
            _videoOfflineText = null;
            _loader = null;
            _videoSurface = null;
        }

        base.Dispose(disposing);
    }

    private void Play()
    {
        _player?.Play();
        _handler.SendEmptyMessage(PlayerMessages.UpdateProgress);
    }
}