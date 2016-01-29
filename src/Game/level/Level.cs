using amulware.Graphics;
using Bearded.Utilities.Math;
using Bearded.Utilities.Tilemaps.Rectangular;
using Centipede.Rendering;
using OpenTK;

namespace Centipede.Game
{
    class Level
    {
        private const float levelPadding = 30;
        private const float gridSize = 5;
        private const float gridSizeHalf = gridSize / 2;
        private const float gridSizeInv = 1f / gridSize;

        private readonly Tilemap<TileInfo> tilemap;
        private readonly float offsetX;
        private readonly float offsetY;

        public Vector2 TileSize { get { return new Vector2(gridSize, gridSize); } }

        public Level(GameState game, float width, float height)
        {
            var w = width + levelPadding * 2;
            var h = height + levelPadding * 2;

            var tilesX = Mathf.CeilToInt(w / gridSize);
            var tilesY = Mathf.CeilToInt(h / gridSize);

            this.tilemap = new Tilemap<TileInfo>(tilesX, tilesY);

            this.offsetX = -tilesX * gridSizeHalf;
            this.offsetY = -tilesY * gridSizeHalf;
        }

        public Tile<TileInfo> this[float x, float y]
        {
            get
            {
                int ty, tx;
                this.positionToTile(x, y, out tx, out ty);
                return new Tile<TileInfo>(this.tilemap, tx, ty);
            }
        }

        public Vector2 GetTileCenter(Tile<TileInfo> tile)
        {
            return this.GetTileTopLeft(tile) + new Vector2(gridSizeHalf, gridSizeHalf);
        }
        public Vector2 GetTileTopLeft(Tile<TileInfo> tile)
        {
            float x, y;
            this.tileToPosition(tile.X, tile.Y, out x, out y);
            return new Vector2(x, y);
        }

        private void positionToTile(float x, float y, out int tx, out int ty)
        {
            tx = (int)((x - this.offsetX) * gridSizeInv);
            ty = (int)((y - this.offsetY) * gridSizeInv);
        }
        private void tileToPosition(int tx, int ty, out float x, out float y)
        {
            x = tx * gridSize + this.offsetX;
            y = ty * gridSize + this.offsetY;
        }

        public void DebugDraw()
        {
            var geo = GeometryManager.Instance.PrimitivesOverlay;

            var argb0 = Color.Red * 0.7f;
            var argb1 = Color.Lime * 0.7f;

            foreach (var tile in this.tilemap)
            {
                geo.Color = (tile.X + tile.Y) % 2 == 0 ? argb0 : argb1;
                var p = this.GetTileTopLeft(tile);
                geo.DrawRectangle(p, this.TileSize - new Vector2(0.2f, 0.2f));
            }
        }
    }
}
