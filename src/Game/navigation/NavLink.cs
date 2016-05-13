
using Bearded.Utilities;
using Bearded.Utilities.SpaceTime;

namespace Centipede.Game
{
    public class NavLink
    {
        public NavQuad From { get; }
        public NavQuad To { get; }

        public Position2 P0 { get; }
        public Position2 P1 { get; }

        public Unit Distance { get; }

        private NavLink(NavQuad from, NavQuad to, Position2 point0, Position2 point1)
        {
            this.From = from;
            this.To = to;

            this.P0 = point0;
            this.P1 = point1;

            this.Distance = (from.Center - to.Center).Length;
        }

        public static void CreatePair(NavQuad quad1, NavQuad quad2, Position2 point0, Position2 point1)
        {
            quad1.Add(new NavLink(quad1, quad2, point0, point1));
            quad2.Add(new NavLink(quad2, quad1, point0, point1));
        }

        public Position2 RandomPoint()
        {
            return Position2.Lerp(this.P0, this.P1, StaticRandom.Float());
        }

    }
}

