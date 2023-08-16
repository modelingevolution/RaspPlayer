using Android.Content.Res;

namespace RaspPlayer.Platforms.Android;

public static class ExtensionsMethods
{
    public static int DpToPx(this int dp)
    {
        return (int)Math.Round(dp * Resources.System.DisplayMetrics.Density);
    }
}