
using amulware.Graphics;
using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;

namespace Centipede.Game
{
    public class NavQuad
    {
        private readonly Position2 p0;
        private readonly Position2 p1;
        private readonly Position2 p2;
        private readonly Position2 p3;

        public NavQuad(Position2 p0, Position2 p1, Position2 p2, Position2 p3)
        {
            this.p0 = p0;
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
        }

        public void Draw(IndexedSurface<PrimitiveVertexData> surface)
        {
            var argb = Color.Yellow;
            surface.AddQuad(
                new PrimitiveVertexData(this.p0.NumericValue.WithZ(), argb),
                new PrimitiveVertexData(this.p1.NumericValue.WithZ(), argb),
                new PrimitiveVertexData(this.p3.NumericValue.WithZ(), argb),
                new PrimitiveVertexData(this.p2.NumericValue.WithZ(), argb)
            );
        }
    }
}

