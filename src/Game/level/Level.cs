﻿using System;
using System.Collections.Generic;
using amulware.Graphics;
using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;
using Bearded.Utilities.Tilemaps.Rectangular;
using Centipede.Rendering;
using OpenTK;
using OpenTK.Graphics.ES20;

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
            return this.GetTileTopLeft(tile) + new Difference2(gridSizeHalf, gridSizeHalf);
        }
        public Position2 GetTileTopLeft(Tile<TileInfo> tile)
        {
            float x, y;
            this.tileToPosition(tile.X, tile.Y, out x, out y);
            return new Position2(x, y);
        }

        private void positionToTile(float x, float y, out int tx, out int ty)
        {
            this.positionToTileSpace(x, y, out x, out y);
            tx = (int)x;
            ty = (int)y;
        }
        private void tileToPosition(int tx, int ty, out float x, out float y)
        {
            this.tileSpaceToPosition(tx, ty, out x, out y);
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

        private void fillBuildingsIntoTiles(GameState game)
        {
            foreach (var building in game.GetList<Building>())
            {
                var tl = building.TopLeft.NumericValue;
                var br = tl + building.Size.NumericValue;
                int x0, x1, y0, y1;
                this.positionToTile(tl.X, tl.Y, out x0, out y0);
                this.positionToTile(br.X, br.Y, out x1, out y1);

                for (int y = y0; y <= y1; y++)
                    for (int x = x0; x <= x1; x++)
                    {
                        this.tilemap[x, y].AddBuilding(building);
                    }
            }
        }

        public IEnumerable<Tile<TileInfo>> CastRay(Ray ray)
        {
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

            var argb0 = Color.Aqua * 0.5f;
            var argb1 = Color.BlueViolet * 0.5f;

            geo.LineWidth = 0.2f;

            foreach (var tile in this.tilemap)
            {
                geo.Color = (tile.X + tile.Y) % 2 == 0 ? argb0 : argb1;
                var p = this.GetTileTopLeft(tile);
                geo.DrawRectangle(p.NumericValue, this.TileSize.NumericValue - new Vector2(0.2f, 0.2f));

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