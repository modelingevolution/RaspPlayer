//using RaspPlayer.VideoSource;

namespace RaspPlayer.Player;

public class Players
{
    private readonly Dictionary<string, NativePlayer> _playersForDevices;
    private readonly string _playerInitParameters;
    //private readonly MultiplexerProxy _multiplexerProxy;

    public Players(/*MultiplexerProxy multiplexerProxy*/)
    {
        _playersForDevices = new Dictionary<string, NativePlayer>();
        _playerInitParameters =  new PlayerInitParametersBuilder().AddDefaults().Build();
      //  _multiplexerProxy = multiplexerProxy;
    }
    
    public NativePlayer GetOrAdd(string device)
    {
        //if (!_multiplexerProxy.IsOnline())
        //{
        //    return new ManagedNativePlayer();
        //}

        if (_playersForDevices.TryGetValue(device, out NativePlayer player))
        {
            return player;
        }
        
      //  VideoSource.VideoSource source = _multiplexerProxy.ForDevice(device);
        
        var newPlayer = new NativePlayer();
       // newPlayer.Open(source.Server, _playerInitParameters, source.Source);
        
        _playersForDevices[device] = newPlayer;
        
        return newPlayer;
    }

    public NativePlayer GetReopenedPlayer(string device)
    {
      // if (!_multiplexerProxy.IsOnline())
      // {
      //     return new ManagedNativePlayer();
      // }
      // 
      // VideoSource.VideoSource source = _multiplexerProxy.ForDevice(device);

        if (_playersForDevices.TryGetValue(device, out NativePlayer player))
        {
            player.Close();
         //   player.Open(source.Server, _playerInitParameters, source.Source);
            
            return player;
        }
        
        var newPlayer = new NativePlayer();
      //  newPlayer.Open(source.Server, _playerInitParameters, source.Source);
        
        _playersForDevices[device] = newPlayer;
        
        return newPlayer;
    }
}