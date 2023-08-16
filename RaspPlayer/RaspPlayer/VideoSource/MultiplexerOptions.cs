namespace RaspPlayer.VideoSource;

public class MultiplexerOptions
{
    public string Host { get; set; }
    public int Port { get; set; }

    public static string SectionName = "Multiplexer";
}