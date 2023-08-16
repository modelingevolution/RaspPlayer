using System.Diagnostics;
using RaspPlayer.Fullscreen;
using RaspPlayer.RaspberryPis;

namespace RaspPlayer.Navigation;

public class NavigationService : INavigationService
{
    private readonly IServiceProvider _services;

    public NavigationService(IServiceProvider services)
    {
        _services = services;
    }

    private INavigation Navigation {
        get
        {
            INavigation navigation = Application.Current?.MainPage?.Navigation;
            if (navigation is not null)
                return navigation;
            else
            {
                if (Debugger.IsAttached)
                    Debugger.Break();
                throw new Exception("Navigation does not exists!");
            }
        }
    }

    public async Task NavigateToFullscreen(RaspberryPi machine)
    {
        var page = ResolvePage<FullscreenVideo>();
        await page.SetMachine(machine);
        await NavigateTo(page);
    }

    public Task NavigateTo<T>(T page) where T : Page
    {
        return Navigation.PushModalAsync(page, true);
    }

    public Task NavigateBack()
    {
        return Navigation.PopModalAsync(true);
    }

    public T ResolvePage<T>()
    {
        var page = _services.GetService<T>();

        if (page is null)
        {
            throw new InvalidOperationException($"Unable to resolve page {typeof(T)}");
        }

        return page;
    }
}