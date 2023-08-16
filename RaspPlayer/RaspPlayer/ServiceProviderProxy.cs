namespace RaspPlayer;

public sealed class ServiceProviderProxy : IServiceProvider
{
    private IServiceProvider _provider;
    public void SetProvider(IServiceProvider provider)
    {
        _provider = provider;
    }

    public object GetService(Type serviceType)
    {
        return _provider.GetService(serviceType);
    }
}