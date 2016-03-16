using System.Collections.Generic;
using Bearded.Utilities.SpaceTime;

namespace Centipede.Game.Generation
{
    class Street
    {
        private readonly List<Intersection> intersections;
        private readonly Comparer<Intersection> comparer;


        public Unit Width { get; private set; }

        public Street(Intersection node1, Intersection node2, Unit width)
        {
            var diffH = node1.Position.X - node2.Position.X;
            var diffV = node1.Position.Y - node2.Position.Y;

            var isEastWest = diffH.Squared > diffV.Squared;
            this.comparer = isEastWest ? eastWestComparer : southNorthComparer;

            this.Width = width;

            if (this.comparer.Compare(node1, node2) > 0)
            {
                this.intersections = new List<Intersection>(3)
                {
                    node2, node1
                };
            }
            else
            {
                this.intersections = new List<Intersection>(3)
                {
                    node1, node2
                };
            }
        }

        public IReadOnlyList<Intersection> Intersections => this.intersections;

        public int AddIntersection(Intersection intersection)
        {
            return this.intersections.AddSorted(intersection, this.comparer);
        }

        private static readonly Comparer<Intersection> eastWestComparer =
            Comparer<Intersection>.Create((i0, i1) => i0.Position.X.CompareTo(i1.Position.X));
        private static readonly Comparer<Intersection> southNorthComparer =
            Comparer<Intersection>.Create((i0, i1) => i0.Position.Y.CompareTo(i1.Position.Y));
    }
}