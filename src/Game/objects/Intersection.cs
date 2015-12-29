using Bearded.Utilities.SpaceTime;

namespace Centipede.Game
{
    sealed class Intersection
    {
        private readonly Position2 position;

        public Intersection(float x, float y)
            : this(new Position2(x, y))
        {

        }
        public Intersection(Unit x, Unit y)
            : this(new Position2(x, y))
        {

        }

        public Intersection(Position2 position)
        {
            this.position = position;
        }

        public Position2 Position { get { return this.position; } }
    }
}