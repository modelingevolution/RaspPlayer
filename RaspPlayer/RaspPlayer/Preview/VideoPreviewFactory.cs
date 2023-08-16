using RaspPlayer.RaspberryPis;

namespace RaspPlayer.Preview;

public class VideoPreviewFactory
{
    private readonly IServiceProvider _services;

    public VideoPreviewFactory(IServiceProvider services)
    {
        _services = services;
    }

    public VideoPreview ForMachine(RaspberryPi machine)
    {
        var preview = _services.GetRequiredService<VideoPreview>();
        preview.SetRasp(machine);
        return preview;
    }
}