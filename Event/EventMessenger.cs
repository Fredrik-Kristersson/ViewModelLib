using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace ViewModelLib.Event
{
    [Export(typeof(IEventMessenger))]
    public class EventMessenger : IEventMessenger
    {
        private readonly IDictionary<Type, List<object>> subscribers =
            new Dictionary<Type, List<object>>();

        public void Publish<T>(T message) where T : ISubscriptionEvent
        {
            if (subscribers.TryGetValue(message.GetType(), out var callbacks))
            {
                foreach (var callback in callbacks)
                {
                    var callbackCast = (Action<T>)callback;
                    callbackCast?.Invoke(message);
                }
            }
        }

        public void Subscribe<T>(Action<T> callback) where T : ISubscriptionEvent
        {
            if (!subscribers.TryGetValue(typeof(T), out List<object> callbacks))
            {
                callbacks = new List<object>();
                subscribers.Add(typeof(T), callbacks);
            }

            callbacks.Add(callback);
        }
    }
}
