namespace RaspPlayer.Device;

public static class DisplaySize
{
    public static double WidthDpi => DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
    public static double HeightDpi => DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
    public static double Width => DeviceDisplay.MainDisplayInfo.Width;
    public static double Height => DeviceDisplay.MainDisplayInfo.Height;
}