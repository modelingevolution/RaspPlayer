using System.Collections.ObjectModel;
using RaspPlayer.UIEvents;
using Microsoft.Extensions.Logging;
using ModelingEvolution.Plumberd;
using ModelingEvolution.Plumberd.Metadata;
using BlazoReactor.EventAggregator;


namespace RaspPlayer.RaspberryPis;

public class RaspberryPis :  IRaspberryPisLookup
{
    private readonly ObservableCollection<RaspberryPi> _rasps;
    private readonly ILogger<RaspberryPis> _logger;

    public RaspberryPis(IMauiEventAggregator eventAggregator, ILogger<RaspberryPis> logger)
    {
        _logger = logger;
        _rasps = new ObservableCollection<RaspberryPi>();
      
    }

    public ObservableCollection<RaspberryPi> Index => _rasps;
    

    public string GetNameByDeviceSerialNumberOrEmpty(string deviceSerialNumber)
    {
        RaspberryPi raspberryPi = _rasps.FirstOrDefault(row => row.DeviceSerialNumber == deviceSerialNumber);

        return raspberryPi?.Name ?? string.Empty;
    }

}