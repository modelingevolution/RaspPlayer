using Android.Runtime;

namespace RaspPlayer.Platforms.Android;

public static class JValueExtensions
{
    public static JValue ToJValue(this string value)
    {
        var javaString = new Java.Lang.String(value);
        return new JValue(javaString);
    }
}