using Android.App;
using Android.Runtime;
using Java.Lang;

namespace RaspPlayer;

[Application]
public class MainApplication : MauiApplication
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
        JavaSystem.LoadLibrary("fanplayer_jni");
    }

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
