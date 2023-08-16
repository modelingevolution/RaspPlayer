using Android.Content;
using Android.Provider;
using ModelingEvolution.Plumberd;

namespace RaspPlayer.Device;

public static partial class Device
{
    public static partial Guid Id()
    {
        Context context = Android.App.Application.Context;
        string id = Settings.Secure.GetString(context.ContentResolver, Settings.Secure.AndroidId);

        return id.ToGuid();
    }
}