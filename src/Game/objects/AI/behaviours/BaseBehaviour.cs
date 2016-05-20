using System;
using System.Collections.Generic;

namespace Centipede.Game.AI
{

    abstract class BaseBehaviour : IBehaviour
    {
        private readonly List<ISubscription> subscriptions = new List<ISubscription>();
        protected CivilianController context { get; private set; }

        public void Start(CivilianController controller)
        {
            this.context = controller;
            this.onStart();
        }

        public void Stop()
        {
            this.onStop();
            this.clearSubscriptions();
            this.context = null;
        }

        public virtual void Update()
        {
        }

        protected virtual void onStart()
        {
        }

        protected virtual void onStop()
        {
        }

        private void clearSubscriptions()
        {
            foreach (var sub in this.subscriptions)
            {
                sub.Unsubscribe();
            }
            this.subscriptions.Clear();
        }

        protected void subscribe(Event @event, Action action)
        {
            this.subscriptions.Add(@event.Subscribe(action));
        }

        protected void subscribe<TEventArgs>(Event<TEventArgs> @event, Action<TEventArgs> action)
        {
            this.subscriptions.Add(@event.Subscribe(action));
        }
    }
}
