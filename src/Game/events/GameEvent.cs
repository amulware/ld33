using System;
using System.Linq;
using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;

namespace Centipede.Game
{
    abstract class GameEvent
    {
        private readonly Squared<Unit> maxRadiusSquared;

        private bool sendStarted;
        private bool sendComplete;
        private int perceivedBy;

        protected GameEvent(Position2 position, Unit visibleRadius, Unit audibleRadius)
        {
            this.Position = position;
            this.VisibleRadius = visibleRadius;
            this.AudibleRadius = audibleRadius;
            this.MaxRadius = visibleRadius > audibleRadius ? visibleRadius : audibleRadius;
            this.maxRadiusSquared = this.MaxRadius.Squared;
        }

        public Position2 Position { get; }
        public Unit VisibleRadius { get; }
        public Unit AudibleRadius { get; }
        public Unit MaxRadius { get; }

        public int PerceivedBy
        {
            get { this.ensureSent(); return this.perceivedBy; }
        }

        public void Send(GameState game)
        {
            this.ensureOnlySendOnce();
            
            this.perceivedBy = game.Level
                .TilesIntersecting(this.Position, this.MaxRadius)
                .Where(tile => tile.IsValid)
                .SelectMany(tile => tile.Value.EventListeners)
                .Count(listener => listener.TryPerceive(this));

            this.sendComplete = true;
        }

        public bool CanBePerceivedAt(Position2 position)
        {
            return this.canBePerceivedFrom(position - this.Position);
        }

        public bool CanBePerceivedAt(Position2 position, Direction2 visionDirection, Angle visionHalfAngle)
        {
            var difference = this.Position - position;
            return this.canBePerceivedFrom(difference) && isInCone(difference, visionDirection, visionHalfAngle);
        }

        private bool canBePerceivedFrom(Difference2 difference)
        {
            return difference.LengthSquared < this.maxRadiusSquared;
        }

        private static bool isInCone(Difference2 difference, Direction2 visionDirection, Angle visionHalfAngle)
        {
            return (difference.Direction - visionDirection).Abs() < visionHalfAngle;
        }

        private void ensureOnlySendOnce()
        {
            if (this.sendStarted)
                throw new Exception("Can only send game event once.");

            this.sendStarted = true;
        }

        private void ensureSent()
        {
            if (!this.sendComplete)
                throw new Exception("Cannot access property before game event has been sent.");
        }

        public abstract void PerceiveBy(IGameEventPerceiver perceiver);
    }
}