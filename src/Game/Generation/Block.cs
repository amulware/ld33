using System.Collections.Generic;
using Bearded.Utilities.SpaceTime;

namespace Centipede.Game.Generation
{
    class Block
    {
        public sealed class Side
        {
            public static readonly Side East = new Side(0);
            public static readonly Side South = new Side(1);
            public static readonly Side West = new Side(2);
            public static readonly Side North = new Side(3);

            private readonly int value;

            private Side(int value)
            {
                this.value = value;
            }
            
            public static implicit operator int(Side side)
            {
                return side.value;
            }
        }

        private readonly Street[] streets;

        public Unit East { get; }
        public Unit West { get; }
        public Unit South { get; }
        public Unit North { get; }

        public Unit Height { get; private set; }
        public Unit Width { get; private set; }

        public Intersection SouthEast { get; private set; }
        public Intersection SouthWest { get; private set; }
        public Intersection NorthEast { get; private set; }
        public Intersection NorthWest { get; private set; }

        public Block(Street streetEast, Street streetSouth, Street streetWest, Street streetNorth,
            Intersection southEast, Intersection southWest, Intersection northEast, Intersection northWest)
        {
            this.SouthEast = southEast;
            this.SouthWest = southWest;
            this.NorthEast = northEast;
            this.NorthWest = northWest;
            this.Children = new List<Block>(4);
            this.streets = new[] { streetEast, streetSouth, streetWest, streetNorth };

            this.East = streetEast.Intersections[0].Position.X;
            this.West = streetWest.Intersections[0].Position.X;
            this.South = streetSouth.Intersections[0].Position.Y;
            this.North = streetNorth.Intersections[0].Position.Y;

            this.Width = this.East - this.West;
            this.Height = this.North - this.South;
        }

        public Street this[Side side] => this.streets[side];

        public List<Block> Children { get; private set; }
        public IReadOnlyList<Street> Streets => this.streets;

        public static Block MakeRoot(float width, float height)
        {
            var x = width / 2;
            var y = height / 2;
            var streetWidth = 5.U();

            var i00 = new Intersection(-x, -y);
            var i10 = new Intersection(x, -y);
            var i01 = new Intersection(-x, y);
            var i11 = new Intersection(x, y);

            var sEast = new Street(i11, i10, streetWidth);
            var sSouth = new Street(i10, i00, streetWidth);
            var sWest = new Street(i00, i01, streetWidth);
            var sNorth = new Street(i01, i11, streetWidth);

            return new Block(
                sEast, sSouth, sWest, sNorth,
                i10, i00, i11, i01
                );
        }
    }
}