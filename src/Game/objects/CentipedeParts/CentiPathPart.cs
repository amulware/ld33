using Bearded.Utilities.SpaceTime;

namespace Centipede.Game.CentipedeParts
{
    struct CentiPathPart
    {
        public CentiPathPart(Position2 position, CentiPathPart previous)
            : this(position, (position - previous.Position).Length)
        { }

        public CentiPathPart(Position2 position, Unit distanceToPrevious)
        {
            this.Position = position;
            this.DistanceToPrevious = distanceToPrevious;
        }

        public Position2 Position { get; }
        public Unit DistanceToPrevious { get; }
    }
}