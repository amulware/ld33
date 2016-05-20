
using System;
using Bearded.Utilities.Collections;

namespace Centipede.Game.AI
{
    class Event
    {
        private readonly DeletableObjectList<Subscription> subscriptions = new DeletableObjectList<Subscription>();

        public void Invoke()
        {
            foreach (var sub in this.subscriptions)
            {
                sub.Invoke();
            }
        }

        public ISubscription Subscribe(Action action)
        {
            var sub = new Subscription(action);
            this.subscriptions.Add(sub);
            return sub;
        }
    }

    class Event<TEventArgs>
    {
        private readonly DeletableObjectList<Subscription<TEventArgs>> subscriptions = new DeletableObjectList<Subscription<TEventArgs>>();

        public void Invoke(TEventArgs args)
        {
            foreach (var sub in this.subscriptions)
            {
                sub.Invoke(args);
            }
        }

        public ISubscription Subscribe(Action<TEventArgs> action)
        {
            var sub = new Subscription<TEventArgs>(action);
            this.subscriptions.Add(sub);
            return sub;
        }
    }
}

