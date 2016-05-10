
using amulware.Graphics;
using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;

namespace Centipede.Game
{
    public class NavQuad
    {
        public Position2 SW { get; }
        public Position2 SE { get; }
        public Position2 NW { get; }
        public Position2 NE { get; }

        public NavQuad(Position2 SW, Position2 SE, Position2 NW, Position2 NE)
        {
            this.SW = SW;
            this.SE = SE;
            this.NW = NW;
            this.NE = NE;
        }

        public void Draw(IndexedSurface<PrimitiveVertexData> surface)
        {
            var argb = Color.Yellow;
            surface.AddQuad(
                new PrimitiveVertexData(this.SW.NumericValue.WithZ(), argb),
                new PrimitiveVertexData(this.SE.NumericValue.WithZ(), argb),
                new PrimitiveVertexData(this.NE.NumericValue.WithZ(), argb),
                new PrimitiveVertexData(this.NW.NumericValue.WithZ(), argb)
            );
        }
    }
}

