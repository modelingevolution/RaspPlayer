namespace MePlayer.VideoView;

public interface IMauiVideoView
{
    void UpdateSource();
    void Show();
    void Hide();
    void Refresh();
    void GoOffline();
}