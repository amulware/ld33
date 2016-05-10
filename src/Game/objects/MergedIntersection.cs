
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Bearded.Utilities.Math.Geometry;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Centipede.Game
{
    class MergedIntersection : GameObject
    {
        private readonly List<Intersection> intersections;

        public MergedIntersection(GameState game, params Intersection[] intersections)
            : base(game)
        {
            this.intersections = intersections.ToList();
            this.Intersections = this.intersections.AsReadOnly();

            this.listAs<MergedIntersection>();
        }


        public Rectangle GetRectangle()
        {
            var p = this.intersections[0].Position.NumericValue;
            var x0 = p.X;
            var x1 = p.X;
            var y0 = p.Y;
            var y1 = p.Y;
            foreach (var intersection in this.intersections)
            {
                var r = intersection.GetRectangle();
                x0 = Math.Min(r.Left, x0);
                x1 = Math.Max(r.Right, x1);
                y0 = Math.Min(r.Top, y0);
                y1 = Math.Max(r.Bottom, y1);
            }
            return new Rectangle(x0, y0, x1 - x0, y1 - y0);
        }

        public IEnumerable<Street> Streets => this.intersections.SelectMany(i => i.Streets);
        public ReadOnlyCollection<Intersection> Intersections { get; }

        public override void Update(TimeSpan elapsedTime)
        {
        }
        public override void Draw()
        {
        }
    }
}

