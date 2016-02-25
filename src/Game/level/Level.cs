using System;
using System.Collections.Generic;
using amulware.Graphics;
using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;
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

        public Difference2 TileSize { get { return new Difference2(gridSize, gridSize); } }

        public Level(GameState game, float width, float height)
        {
            var w = width + levelPadding * 2;
            var h = height + levelPadding * 2;

            var tilesX = Mathf.CeilToInt(w / gridSize);
            var tilesY = Mathf.CeilToInt(h / gridSize);

            this.tilemap = new Tilemap<TileInfo>(tilesX, tilesY);

            this.offsetX = -tilesX * gridSizeHalf;
            this.offsetY = -tilesY * gridSizeHalf;

            this.fillTiles();

            this.fillBuildingsIntoTiles(game);
        }

        private void fillTiles()
        {
            foreach (var tile in this.tilemap)
            {
                this.tilemap[tile] = new TileInfo();
            }
        }

        public Tile<TileInfo> this[Position2 point]
        {
            get
            {
                var p = point.NumericValue;
                int ty, tx;
                this.positionToTile(p.X, p.Y, out tx, out ty);
                return new Tile<TileInfo>(this.tilemap, tx, ty);
            }
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

        public Position2 GetTileCenter(Tile<TileInfo> tile)
        {
            float x, y;
            this.tileToCenterPosition(tile.X, tile.Y, out x, out y);
            return new Position2(x, y);
        }
        public Position2 GetTileTopLeft(Tile<TileInfo> tile)
        {
            float x, y;
            this.tileSpaceToPosition(tile.X, tile.Y, out x, out y);
            return new Position2(x, y);
        }

        private void positionToTile(float x, float y, out int tx, out int ty)
        {
            this.positionToTileSpace(x, y, out x, out y);
            tx = (int)x;
            ty = (int)y;
        }
        private void tileToCenterPosition(float tx, float ty, out float x, out float y)
        {
            this.tileSpaceToPosition(tx, ty, out x, out y);
            x += gridSizeHalf;
            y += gridSizeHalf;
        }

        private void positionToTileSpace(float x, float y, out float tx, out float ty)
        {
            tx = (x - this.offsetX) * gridSizeInv;
            ty = (y - this.offsetY) * gridSizeInv;
        }
        private void tileSpaceToPosition(float tx, float ty, out float x, out float y)
        {
            x = tx * gridSize + this.offsetX;
            y = ty * gridSize + this.offsetY;
        }

        public IEnumerable<Tile<TileInfo>> TilesIn(TileRectangle rectangle)
        {
            var x0 = rectangle.X0;
            var y0 = rectangle.Y0;
            var x1 = rectangle.X1;
            var y1 = rectangle.Y1;

            for (var y = y0; y <= y1; y++)
                for (var x = x0; x <= x1; x++)
                {
                    yield return new Tile<TileInfo>(this.tilemap, x, y);
                }
        }

        public TileRectangle RectangleIntersecting(Position2 point, Difference2 size)
        {
            var tl = point.NumericValue;
            var br = tl + size.NumericValue;
            int x0, x1, y0, y1;
            this.positionToTile(tl.X, tl.Y, out x0, out y0);
            this.positionToTile(br.X, br.Y, out x1, out y1);

            return new TileRectangle(x0, y0, x1, y1);
        }

        public IEnumerable<Tile<TileInfo>> TilesIntersecting(Position2 point, Difference2 size)
        {
            return this.TilesIn(this.RectangleIntersecting(point, size));
        }

        public IEnumerable<Tile<TileInfo>> TilesIntersecting(Position2 center, Unit radius)
        {
            var r = radius.NumericValue;
            var rSquared = r * r;
            var centerV = center.NumericValue;
            var tl = centerV + new Vector2(-r);
            var br = centerV + new Vector2(r);
            int x0, x1, y0, y1;
            this.positionToTile(tl.X, tl.Y, out x0, out y0);
            this.positionToTile(br.X, br.Y, out x1, out y1);

            float tileCenterX0, tileCenterY;

            this.tileToCenterPosition(x0, y0, out tileCenterX0, out tileCenterY);

            var tileCenterX1 = tileCenterX0 + gridSize * (x1 - x0);

            var geo = GeometryManager.Instance.PrimitivesOverlay;
            
            for (var y = y0; y <= y1; y++)
            {
                geo.Color = Color.Red * 0.75f;

                var xStart = x0;
                var tileCenterX = tileCenterX0;
                while (xStart <= x1)
                {
                    geo.DrawRectangle(tileCenterX - gridSizeHalf, tileCenterY - gridSizeHalf, gridSize, gridSize);
                    if (rectIntersectsCircle(
                        new Vector2(tileCenterX, tileCenterY),
                        new Vector2(gridSizeHalf, gridSizeHalf),
                        centerV, rSquared))
                    {
                        break;
                    }

                    tileCenterX += gridSize;
                    xStart += 1;
                }

                var xEnd = x1;
                tileCenterX = tileCenterX1;
                while (xEnd > xStart)
                {
                    geo.DrawRectangle(tileCenterX - gridSizeHalf, tileCenterY - gridSizeHalf, gridSize, gridSize);
                    if (rectIntersectsCircle(
                        new Vector2(tileCenterX, tileCenterY),
                        new Vector2(gridSizeHalf, gridSizeHalf),
                        centerV, rSquared))
                    {
                        break;
                    }

                    tileCenterX -= gridSize;
                    xEnd -= 1;
                }

                for (var x = xStart; x <= xEnd; x++)
                {
                    geo.Color = Color.Green * 0.75f;
                    geo.DrawRectangle(this.GetTileTopLeft(new Tile<TileInfo>(this.tilemap, x, y)).NumericValue, this.TileSize.NumericValue);


                    yield return new Tile<TileInfo>(this.tilemap, x, y);
                }

                tileCenterY += gridSize;
            }
        }

        private static bool rectIntersectsCircle(Vector2 rectCenter, Vector2 rectHalfSize, Vector2 center, float radiusSquared)
        {
            var diff = rectCenter - center;
            var diffPositive = new Vector2(Math.Abs(diff.X), Math.Abs(diff.Y));
            var closest = diffPositive - rectHalfSize;
            var closestPositive = new Vector2(Math.Max(closest.X, 0), Math.Max(closest.Y, 0));
            return closestPositive.LengthSquared <= radiusSquared;
        }

        private void fillBuildingsIntoTiles(GameState game)
        {
            foreach (var building in game.GetList<Building>())
            {
                foreach (var tile in this.TilesIntersecting(building.TopLeft, building.Size))
                {
                    tile.Value.AddBuilding(building);
                }
            }
        }

        public IEnumerable<Tile<TileInfo>> CastRay(Ray ray)
        {
            // check http://playtechs.blogspot.nl/2007/03/raytracing-on-grid.html
            // for possible optimisations
            var start = ray.Start.NumericValue;
            var diff = ray.Direction.NumericValue;
            float startX, startY;
            this.positionToTileSpace(start.X, start.Y, out startX, out startY);
            int x1, y1;
            this.positionToTile(start.X + diff.X, start.Y + diff.Y, out x1, out y1);
            var x0 = (int)startX;
            var y0 = (int)startY;

            var tileDistanceX = x1 - x0;
            var tileDistanceY = y1 - y0;

            var count = 1 + Math.Abs(tileDistanceX) + Math.Abs(tileDistanceY);

            yield return new Tile<TileInfo>(this.tilemap, x0, y0);

            if (count <= 2)
            {
                if (count == 2)
                {
                    yield return new Tile<TileInfo>(this.tilemap, x1, y1);
                }
                yield break;
            }

            var tileX = x0;
            var tileY = y0;

            var tileStepX = Math.Sign(tileDistanceX);
            var tileStepY = Math.Sign(tileDistanceY);
            
            float intersectXStep, intersectYStep;
            float nextIntersectX, nextIntersectY;

            getRayIntersectParameters(startX, diff.X, out nextIntersectX, out intersectXStep);
            getRayIntersectParameters(startY, diff.Y, out nextIntersectY, out intersectYStep);

            for (int i = 2; i < count; i++)
            {
                if (nextIntersectX < nextIntersectY)
                {
                    tileX += tileStepX;
                    nextIntersectX += intersectXStep;
                }
                else
                {
                    tileY += tileStepY;
                    nextIntersectY += intersectYStep;
                }
                yield return new Tile<TileInfo>(this.tilemap, tileX, tileY);
            }

            yield return new Tile<TileInfo>(this.tilemap, x1, y1);
        }

        private static void getRayIntersectParameters(float start, float diff,
            out float nextIntersectF, out float intersectFStep)
        {
            if (diff == 0)
            {
                intersectFStep = float.NaN;
                nextIntersectF = float.PositiveInfinity;
                return;
            }

            var startFraction = start - (float)Math.Floor(start);
            if (diff > 0)
            {
                intersectFStep = 1 / diff;
                nextIntersectF = (1 - startFraction) * intersectFStep;
            }
            else
            {
                intersectFStep = -1 / diff;
                nextIntersectF = startFraction * intersectFStep;
            }
        }

        public void DebugDraw()
        {
            var geo = GeometryManager.Instance.PrimitivesOverlay;

            var argb0 = Color.Aqua * 0.1f;
            var argb1 = Color.BlueViolet * 0.1f;

            geo.Color = argb1;

            geo.LineWidth = 0.2f;

            foreach (var tile in this.tilemap)
            {
                //geo.Color = (tile.X + tile.Y) % 2 == 0 ? argb0 : argb1;
                if((tile.X + tile.Y) % 2 == 0)
                    continue;

                var p = this.GetTileTopLeft(tile);
                geo.DrawRectangle(p.NumericValue, this.TileSize.NumericValue);

                //geo.Color = Color.Blue;
                //foreach (var building in tile.Value.Buildings)
                //{
                //    geo.DrawLine(this.GetTileCenter(tile).NumericValue,
                //        (building.TopLeft + building.Size * 0.5f).NumericValue);   
                //}
            }
        }
    }
}
