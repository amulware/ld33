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

        public void DrawBuilding(Vector2 topLeft, Vector2 size, float height, float interiorAlpha)
        {
            this.drawOutside(topLeft, size, height);


            if (interiorAlpha > 0)
            {
                this.drawInside(topLeft, size, height, interiorAlpha);
            }
            else
            {
                this.drawTop(topLeft, topLeft + size, height);   
            }
        }

        private void drawInside(Vector2 topLeft, Vector2 size, float height, float alpha)
        {
            const float wallWidth = 0.5f;

            var topRight = topLeft + new Vector2(size.X, 0);
            var bottomRight = topLeft + size;
            var bottomLeft = topLeft + new Vector2(0, size.Y);

            var topLeftInner = topLeft + new Vector2(wallWidth, wallWidth);
            var topRightInner = topRight + new Vector2(-wallWidth, wallWidth);
            var bottomRightInner = bottomRight + new Vector2(-wallWidth, -wallWidth);
            var bottomLeftInner = bottomLeft + new Vector2(wallWidth, -wallWidth);

            this.drawFace(topRightInner, topLeftInner, height, new Vector2(0, 1));
            this.drawFace(bottomRightInner, topRightInner, height, new Vector2(-1, 0));
            this.drawFace(bottomLeftInner, bottomRightInner, height, new Vector2(0, -1));
            this.drawFace(topLeftInner, bottomLeftInner, height, new Vector2(1, 0));

            this.drawWallInterior(topLeft, topRight, topRightInner, topLeftInner, height);
            this.drawWallInterior(topRight, bottomRight, bottomRightInner, topRightInner, height);
            this.drawWallInterior(bottomRight, bottomLeft, bottomLeftInner, bottomRightInner, height);
            this.drawWallInterior(bottomLeft, topLeft, topLeftInner, bottomLeftInner, height);

            this.drawTop(topLeftInner, bottomRightInner, height, 1 - alpha);   
        }

        private void drawWallInterior(Vector2 c0, Vector2 c1, Vector2 c2, Vector2 c3, float height)
        {
            var n = new Vector3(0, 0, 1);
            var uv = new Vector2();

            this.surface.AddQuad(
                new BuildingVertex(c0.WithZ(height), n, uv),
                new BuildingVertex(c1.WithZ(height), n, uv),
                new BuildingVertex(c2.WithZ(height), n, uv),
                new BuildingVertex(c3.WithZ(height), n, uv)
                );
        }


        private void drawOutside(Vector2 topLeft, Vector2 size, float height)
        {
            var topRight = topLeft + new Vector2(size.X, 0);
            var bottomRight = topLeft + size;
            var bottomLeft = topLeft + new Vector2(0, size.Y);

            this.drawFace(topLeft, topRight, height, new Vector2(0, -1));
            this.drawFace(topRight, bottomRight, height, new Vector2(1, 0));
            this.drawFace(bottomRight, bottomLeft, height, new Vector2(0, 1));
            this.drawFace(bottomLeft, topLeft, height, new Vector2(-1, 0));
        }

        private void drawTop(Vector2 topLeft, Vector2 bottomRight, float height, float alpha = 1)
        {
            if (alpha < 1)
                return;

            var n = new Vector3(0, 0, 1);
            var uv = new Vector2();

            this.surface.AddQuad(
                new BuildingVertex(new Vector3(topLeft.X, topLeft.Y, height), n, uv, alpha),
                new BuildingVertex(new Vector3(bottomRight.X, topLeft.Y, height), n, uv, alpha),
                new BuildingVertex(new Vector3(bottomRight.X, bottomRight.Y, height), n, uv, alpha),
                new BuildingVertex(new Vector3(topLeft.X, bottomRight.Y, height), n, uv, alpha)
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