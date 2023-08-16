using RaspPlayer.UIEvents;

namespace RaspPlayer;

public partial class App : Application
{
	public App(AppShell appShell, IMauiEventAggregator mauiEventAggregator)
	{
		InitializeComponent();

        MainPage = appShell;
    }
}
