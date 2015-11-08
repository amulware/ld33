using Bearded.Utilities.SpaceTime;

namespace Centipede.Game.CentipedeParts
{
    struct CentiPathPart
    {
        private readonly Position2 position;
        private readonly Radius distanceToPrevious;

        public CentiPathPart(Position2 position, CentiPathPart previous)
        {
            this.position = position;
            this.distanceToPrevious = (position - previous.position).Length;
        }

        public CentiPathPart(Position2 position, Radius distanceToPrevious)
        {
            this.position = position;
            this.distanceToPrevious = distanceToPrevious;
        }

        public Position2 Position { get { return this.position; } }
        public Radius DistanceToPrevious { get { return this.distanceToPrevious; } }
    }
}