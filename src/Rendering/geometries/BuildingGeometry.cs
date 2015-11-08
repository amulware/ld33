using amulware.Graphics;
using Bearded.Utilities.Math;
using OpenTK;

namespace Centipede.Rendering
{
    class BuildingGeometry
    {
        private readonly IndexedSurface<BuildingVertex> surface;

        public BuildingGeometry(IndexedSurface<BuildingVertex> surface)
        {
            this.surface = surface;
        }

        public void DrawBuilding(Vector2 topLeft, Vector2 size, float height)
        {
            var topRight = topLeft + new Vector2(size.X, 0);
            var bottomRight = topLeft + size;
            var bottomLeft = topLeft + new Vector2(0, size.Y);

            this.drawFace(topLeft, topRight, height, new Vector2(0, -1));
            this.drawFace(topRight, bottomRight, height, new Vector2(1, 0));
            this.drawFace(bottomRight, bottomLeft, height, new Vector2(0, 1));
            this.drawFace(bottomLeft, topLeft, height, new Vector2(-1, 0));
            this.drawTop(topLeft, bottomRight, height);
        }

        private void drawTop(Vector2 topLeft, Vector2 bottomRight, float height)
        {
            var n = new Vector3(0, 0, 1);
            var uv = new Vector2();

            this.surface.AddQuad(
                new BuildingVertex(new Vector3(topLeft.X, topLeft.Y, height), n, uv),
                new BuildingVertex(new Vector3(bottomRight.X, topLeft.Y, height), n, uv),
                new BuildingVertex(new Vector3(bottomRight.X, bottomRight.Y, height), n, uv),
                new BuildingVertex(new Vector3(topLeft.X, bottomRight.Y, height), n, uv)
                );
        }

        private void drawFace(Vector2 from, Vector2 to, float height, Vector2 normal)
        {
            var n = normal.WithZ();
            var uv = new Vector2();

            this.surface.AddQuad(
                new BuildingVertex(from.WithZ(0), n, uv), 
                new BuildingVertex(to.WithZ(0), n, uv), 
                new BuildingVertex(to.WithZ(height), n, uv), 
                new BuildingVertex(from.WithZ(height), n, uv)
                );
        }
    }
}