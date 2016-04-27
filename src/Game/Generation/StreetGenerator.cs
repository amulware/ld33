using System;
using System.Collections.Generic;
using Bearded.Utilities;
using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;

namespace Centipede.Game.Generation
{
    class StreetGenerator
    {
        public void Generate(GameState game, float width, float height)
        {
            var root = this.makeRootSquare(width, height);

            this.fillBlock(root);

            this.fillGame(root, game);
        }

        private static readonly Unit minBlockSize = 20.U();

        private void fillBlock(Block block)
        {
            var canSplitNS = block.Width > minBlockSize;
            var canSplitEW = block.Height > minBlockSize;

            if (!(canSplitNS || canSplitEW))
                return;

            if (canSplitNS && canSplitEW)
            {
                this.splitCross(block);
            }
            else if (canSplitNS)
            {
                this.splitNorthSouth(block);
            }
            else
            {
                this.splitEastWest(block);
            }



            foreach (var child in block.Children)
            {
                this.fillBlock(child);
            }
        }

        private void splitEastWest(Block block)
        {
            Unit streetWidth;
            var y = this.divide(block.South, block.North, out streetWidth);

            var nodeEast = new Intersection(block.East, y);
            var nodeWest = new Intersection(block.West, y);

            var streetEast = block[Block.Side.East];
            var streetWest = block[Block.Side.West];
            var streetSouth = block[Block.Side.South];
            var streetNorth = block[Block.Side.North];

            streetEast.AddIntersection(nodeEast);
            streetWest.AddIntersection(nodeWest);

            var streetSplit = new Street(nodeEast, nodeWest, streetWidth);

            var childS = new Block(streetEast, streetSouth, streetWest, streetSplit, block.SouthEast, block.SouthWest, nodeEast, nodeWest);
            var childN = new Block(streetEast, streetSplit, streetWest, streetNorth, nodeEast, nodeWest, block.NorthEast, block.NorthWest);

            block.Children.Add(childS);
            block.Children.Add(childN);
        }

        private void splitNorthSouth(Block block)
        {
            Unit streetWidth;
            var x = this.divide(block.East, block.West, out streetWidth);

            var nodeSouth = new Intersection(x, block.South);
            var nodeNorth = new Intersection(x, block.North);

            var streetEast = block[Block.Side.East];
            var streetWest = block[Block.Side.West];
            var streetSouth = block[Block.Side.South];
            var streetNorth = block[Block.Side.North];

            streetSouth.AddIntersection(nodeSouth);
            streetNorth.AddIntersection(nodeNorth);

            var streetSplit = new Street(nodeSouth, nodeNorth, streetWidth);

            var childE = new Block(streetEast, streetSouth, streetSplit, streetNorth, block.SouthEast, nodeSouth, block.NorthEast, nodeNorth);
            var childW = new Block(streetSplit, streetSouth, streetWest, streetNorth, nodeSouth, block.SouthWest, nodeNorth, block.NorthWest);

            block.Children.Add(childE);
            block.Children.Add(childW);
        }

        private void splitCross(Block block)
        {
            Unit streetWidthNS, streetWidthEW;
            var x = this.divide(block.East, block.West, out streetWidthEW);
            var y = this.divide(block.South, block.North, out streetWidthNS);

            var nodeCenter = new Intersection(x, y);
            var nodeEast = new Intersection(block.East, y);
            var nodeWest = new Intersection(block.West, y);
            var nodeSouth = new Intersection(x, block.South);
            var nodeNorth = new Intersection(x, block.North);

            var streetEast = block[Block.Side.East];
            var streetWest = block[Block.Side.West];
            var streetSouth = block[Block.Side.South];
            var streetNorth = block[Block.Side.North];

            streetEast.AddIntersection(nodeEast);
            streetWest.AddIntersection(nodeWest);
            streetSouth.AddIntersection(nodeSouth);
            streetNorth.AddIntersection(nodeNorth);

            var streetToEast = new Street(nodeCenter, nodeEast, streetWidthEW);
            var streetToWest = new Street(nodeCenter, nodeWest, streetWidthEW);
            var streetToSouth = new Street(nodeCenter, nodeSouth, streetWidthNS);
            var streetToNorth = new Street(nodeCenter, nodeNorth, streetWidthNS);

            var childSE = new Block(streetEast, streetSouth, streetToSouth, streetToEast, block.SouthEast, nodeSouth, nodeEast, nodeCenter);
            var childSW = new Block(streetToSouth, streetSouth, streetWest, streetToWest, nodeSouth, block.SouthWest, nodeCenter, nodeWest);
            var childNE = new Block(streetToNorth, streetToWest, streetWest, streetNorth, nodeCenter, nodeWest, nodeNorth, block.NorthWest);
            var childNW = new Block(streetEast, streetToEast, streetToNorth, streetNorth, nodeEast, nodeCenter, block.NorthEast, nodeNorth);

            block.Children.Add(childSE);
            block.Children.Add(childSW);
            block.Children.Add(childNE);
            block.Children.Add(childNW);
        }

        private Unit divide(Unit p0, Unit p1, out Unit streetWidth)
        {
            if(p0 > p1)
                Do.Swap(ref p0, ref p1);

            var d = p1 - p0;

            var sw = d.NumericValue.Sqrted() * 0.3f;

            streetWidth = Math.Floor(sw).Clamped(1.5f, 5).U();

            var ret = p0 + d * StaticRandom.Float(0.3f, 0.7f);

            var offset = streetWidth + minBlockSize * 0.5f;

            var ret2 = ret.NumericValue.Clamped((p0 + offset).NumericValue, (p1 - offset).NumericValue).U();
            
            return ret2;
        }

        private void fillGame(Block root, GameState game)
        {
            var blocks = new Stack<Block>();
            blocks.Push(root);

            var generatedStreets = new HashSet<Street>();

            while (blocks.Count > 0)
            {
                var b = blocks.Pop();

                if (b.Children.Count == 0)
                {
                    var e = b[Block.Side.East].Width;
                    var w = b[Block.Side.West].Width;
                    var s = b[Block.Side.South].Width;
                    var n = b[Block.Side.North].Width;

                    new Building(game,
                        b.SouthWest.Position + new Difference2(w, s),
                        b.NorthEast.Position - b.SouthWest.Position
                        - new Difference2(w + e, n + s));
                }
                else
                {
                    foreach (var child in b.Children)
                    {
                        blocks.Push(child);
                    }   
                }

                foreach (var street in b.Streets)
                {
                    if (!generatedStreets.Add(street))
                    {
                        continue;
                    }
                    var w = street.Width;
                    var intersections = street.Intersections;
                    for (int i = 1; i < intersections.Count; i++)
                    {
                        new Game.Street(game, intersections[i - 1], intersections[i], w);
                    }
                }
            }

            this.mergeIntersections(game);
        }

        private void mergeIntersections(GameState game)
        {
            var intersectionsVisited = new HashSet<Intersection>();
            
            foreach (var street in game.GetList<Game.Street>())
            {
                var sawNode1 = intersectionsVisited.Add(street.Node1);
                var sawNode2 = intersectionsVisited.Add(street.Node2);

                var rect1 = street.Node1.GetRectangle();
                var rect2 = street.Node2.GetRectangle();

                var intersect = rect1.Intersects(rect2);

                if (intersect)
                {
                    street.Delete();

                    new MergedIntersection(game, street.Node1, street.Node2);
                }
                else
                {
                    if (!sawNode1)
                    {
                        new MergedIntersection(game, street.Node1);
                    }
                    if (!sawNode2)
                    {
                        new MergedIntersection(game, street.Node2);
                    }
                }

            }
        }


        private Block makeRootSquare(float width, float height)
        {
            return Block.MakeRoot(width, height);
        }
    }
}