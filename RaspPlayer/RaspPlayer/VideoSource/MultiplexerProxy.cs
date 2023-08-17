using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace RaspPlayer.VideoSource;

public class MultiplexerProxy
{
    private readonly MultiplexerOptions _multiplexerOptions;
    private readonly ILogger<MultiplexerProxy> _logger;
    private DateTime? _validUntill;
    private bool _lastValue;
    public MultiplexerProxy(MultiplexerOptions options, ILogger<MultiplexerProxy> logger)
    {
        _logger = logger;
        _multiplexerOptions = options;
    }
    
    public VideoSource ForDevice(string device)
    {
        var multiplexerAddress = $"tcp://{_multiplexerOptions.Host}:{_multiplexerOptions.Port}";
        return new VideoSource(multiplexerAddress, device);
    }

    public bool IsOnline()
    {
        if (_validUntill != null && DateTime.Now < _validUntill.Value)
            return _lastValue;

        _validUntill = DateTime.Now.AddSeconds(10);
        using var client = new TcpClient();
        client.ReceiveTimeout = 3;
        try
        {
            var result = client.BeginConnect(_multiplexerOptions.Host, _multiplexerOptions.Port,null,null);
            var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));
            if (!success)
            {
                throw new Exception("Failed to connect.");
            }
            _lastValue = true;
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Multiplexer is not responding");
        }
        finally
        {
            client.Close();
        }
        _lastValue = false;
        return false;
    }
}