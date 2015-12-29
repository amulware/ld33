using System.Collections.Generic;
using Bearded.Utilities.SpaceTime;

namespace Centipede.Game
{
    sealed class Intersection
    {
        private readonly Position2 position;
        private readonly List<Street> streets = new List<Street>();
        private readonly IReadOnlyList<Street> streetsAsReadOnly;

        public Intersection(float x, float y)
            : this(new Position2(x, y)) { }
        public Intersection(Unit x, Unit y)
            : this(new Position2(x, y)) { }

        public Intersection(Position2 position)
        {
            this.position = position;
            this.streetsAsReadOnly = this.streets.AsReadOnly();
        }

        public Position2 Position { get { return this.position; } }
        public IReadOnlyList<Street> Streets { get { return this.streetsAsReadOnly; } }

        public void AddStreet(Street street)
        {
            this.streets.Add(street);
        }
    }
}