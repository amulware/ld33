
using System;
using Bearded.Utilities.Collections;

namespace Centipede.Game.AI
{
    interface ISubscription
    {
        void Unsubscribe();
    }

    class Subscription : IDeletable, ISubscription
    {
        public bool Deleted { get; private set; }

        private readonly Action action;

        public Subscription(Action action)
        {
            this.action = action;
        }

        public void Invoke()
        {
            this.action();
        }

        public void Unsubscribe()
        {
            this.Deleted = true;
        }
    }

    class Subscription<TEventArgs> : IDeletable, ISubscription
    {
        public bool Deleted { get; private set; }

        private readonly Action<TEventArgs> action;

        public Subscription(Action<TEventArgs> action)
        {
            this.action = action;
        }

        public void Invoke(TEventArgs args)
        {
            this.action(args);
        }

        public void Unsubscribe()
        {
            this.Deleted = true;
        }
    }
    
}
