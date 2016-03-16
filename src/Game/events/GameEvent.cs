using System;
using System.Linq;
using Bearded.Utilities.SpaceTime;

namespace Centipede.Game
{
    class GameEvent : IGameEvent
    {
        private bool sent;

        public GameEvent(Position2 position, Unit visibleRadius, Unit audibleRadius)
        {
            this.Position = position;
            this.VisibleRadius = visibleRadius;
            this.AudibleRadius = audibleRadius;
            this.MaxRadius = visibleRadius > audibleRadius ? visibleRadius : audibleRadius;
        }

        public Position2 Position { get; }
        public Unit VisibleRadius { get; }
        public Unit AudibleRadius { get; }
        public Unit MaxRadius { get; }

        public void Send(GameState game)
        {
            this.ensureOnlySendOnce();

            foreach (var listener in game.Level
                .TilesIntersecting(this.Position, this.MaxRadius)
                .Where(tile => tile.IsValid)
                .SelectMany(tile => tile.Value.EventListeners))
            {
                listener.TryPerceive(this);
            }
        }

        private void ensureOnlySendOnce()
        {
            if (this.sent)
                throw new Exception("Can only send game event once.");

            this.sent = true;
        }
    }
}