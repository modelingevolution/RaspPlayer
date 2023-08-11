using System.Text;

namespace RaspPlayer.Player;

public class PlayerInitParametersBuilder
{
    private readonly Dictionary<string, string> _parameters;

    public PlayerInitParametersBuilder()
    {
        _parameters = new Dictionary<string, string>();
    }

    public PlayerInitParametersBuilder Add(string name, string value)
    {
        _parameters[name] = value;
        return this;
    }

    public PlayerInitParametersBuilder AddDefaults()
    {
        Add("video_hwaccel", "1");
        Add("init_timeout", "2000");
        Add("auto_reconnect", "2000");
        Add("audio_bufpktn", "4");
        Add("video_bufpktn", "0");
        Add("avts_syncmode", "3");
        Add("video_framerate", "24");
        
        return this;
    }
    
    public string Build()
    {
        var parametersBuilder = new StringBuilder();

        foreach (KeyValuePair<string, string> parameter in _parameters)
        {
            parametersBuilder.Append(parameter.Key);
            parametersBuilder.Append('=');
            parametersBuilder.Append(parameter.Value);
            parametersBuilder.Append(';');
        }

        return parametersBuilder.ToString();
    }
}