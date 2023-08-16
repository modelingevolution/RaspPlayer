
using BlazoReactor.EventAggregator;

namespace RaspPlayer.UIEvents;

public class MauiEventAggregator : IMauiEventAggregator
{
    private readonly Dictionary<Type, EventBase> _events;
    private readonly SynchronizationContext _syncContext = SynchronizationContext.Current;

    public MauiEventAggregator()
    {
        _events = new Dictionary<Type, EventBase>();
    }
    
    public PubSubEvent<TEventType> GetEvent<TEventType>() where TEventType : new()
    {
        lock (_events)
        {
            if (_events.TryGetValue(typeof (TEventType), out EventBase eventBase))
                return (PubSubEvent<TEventType>) eventBase;
            var pubSubEvent = new PubSubEvent<TEventType>
            {
                SynchronizationContext = _syncContext
            };
            _events[typeof (TEventType)] = pubSubEvent;
            return pubSubEvent;
        }
    }
}