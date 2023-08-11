namespace RaspPlayer.Player;

public static class PlayerParameters
{
    public const int MediaDuration = 0x1000 + 0;
    public const int MediaPosition = 0x1000 + 1;
    public const int VideoWidth = 0x1000 + 2;
    public const int VideoHeight = 0x1000 + 3;
    public const int VideoMode    = 0x1000 + 4;
    public const int AudioVolume = 0x1000 + 5;
    public const int PlaySpeed = 0x1000 + 6;
    public const int VisualEffect = 0x1000 + 7;
    public const int AsyncTimeDiff = 0x1000 + 8;
    public const int PlayerCallback   = 0x1000 + 9;
}