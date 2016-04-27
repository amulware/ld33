using System;
using System.Collections.Generic;
using Bearded.Utilities.Math.Geometry;
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

        public void RemoveStreet(Street street)
        {
            this.streets.Remove(street);
        }

        public Rectangle GetRectangle()
        {
            var w = 0.U();
            var h = 0.U();

            foreach (var street in this.streets)
            {
                var otherNode = street.OtherNode(this);
                if (otherNode.Position.X == this.Position.X)
                {
                    if (street.Width > w)
                        w = street.Width;
                }
                else if (otherNode.Position.Y == this.Position.Y)
                {
                    if (street.Width > h)
                        h = street.Width;
                }
                else
                {
                    throw new Exception("Bad.");
                }
            }
            var wf = w.NumericValue;
            var hf = h.NumericValue;
            var xy = this.Position.NumericValue;
            return new Rectangle(xy.X - wf, xy.Y - hf, wf * 2, hf * 2);
        }
   }
}