using System.Collections.Generic;
using Bearded.Utilities.SpaceTime;

namespace Centipede.Game
{
    sealed class Intersection
    {
        private readonly List<Street> streets = new List<Street>();

        public Intersection(float x, float y)
            : this(new Position2(x, y)) { }
        public Intersection(Unit x, Unit y)
            : this(new Position2(x, y)) { }

        public Intersection(Position2 position)
        {
            this.Position = position;
            this.Streets = this.streets.AsReadOnly();
        }

        public Position2 Position { get; }
        public IReadOnlyList<Street> Streets { get; }

        public void AddStreet(Street street)
        {
            this.streets.Add(street);
        }
    }
}