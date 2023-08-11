namespace RaspPlayer.Player;

public static class PlayerMessages
{
    public const int UpdateProgress = 1;
    public const int UpdateViewSize = 2;
    public const int OpenDone = ('O' << 24) | ('P' << 16) | ('E' << 8) | ('N' << 0);
    public const int OpenFailed = ('F' << 24) | ('A' << 16) | ('I' << 8) | ('L' << 0);
    public const int PlayProgress = ('R' << 24) | ('U' << 16) | ('N' << 8) | (' ' << 0);
    public const int PlayCompleted = ('E' << 24) | ('N' << 16) | ('D' << 8) | (' ' << 0);
    public const int TakeSnapshot = ('S' << 24) | ('N' << 16) | ('A' << 8) | ('P' << 0);
    public const int StreamConnected = ('C' << 24) | ('N' << 16) | ('C' << 8) | ('T' << 0);
    public const int StreamDisconnect = ('D' << 24) | ('I' << 16) | ('S' << 8) | ('C' << 0);
    public const int VideoResized = ('S' << 24) | ('I' << 16) | ('Z' << 8) | ('E' << 0);
}