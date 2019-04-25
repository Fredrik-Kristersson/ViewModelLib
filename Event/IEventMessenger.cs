using System;

namespace ViewModelLib.Event
{
    public interface IEventMessenger
    {
        void Subscribe<T>(Action<T> callback) where T : ISubscriptionEvent;

        void Publish<T>(T message) where T : ISubscriptionEvent;

    }
}
