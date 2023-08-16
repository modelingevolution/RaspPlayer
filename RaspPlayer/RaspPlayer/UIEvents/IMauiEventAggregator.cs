
using BlazoReactor.EventAggregator;

namespace RaspPlayer.UIEvents;

public interface IMauiEventAggregator
{
    PubSubEvent<TEventType> GetEvent<TEventType>() where TEventType : new();
}