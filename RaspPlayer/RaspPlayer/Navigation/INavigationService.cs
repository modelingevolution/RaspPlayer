using RaspPlayer.RaspberryPis;

namespace RaspPlayer.Navigation;

public interface INavigationService
{
    Task NavigateToFullscreen(RaspberryPi raspberryPi);
    Task NavigateTo<T>(T page) where T : Page;
    Task NavigateBack();
    T ResolvePage<T>();
}