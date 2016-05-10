using System;
using System.Collections.Generic;
using System.Linq;
using Bearded.Utilities;
using Bearded.Utilities.SpaceTime;

namespace Centipede.Game.Generation
{
    class NavMeshGenerator
    {
        private readonly GameState game;

        public NavMeshGenerator(GameState game)
        {
            this.game = game;
        }

        public void Generate()
        {
            var navMesh = new NavMesh(this.game);

            var intersectionMap = new Dictionary<Intersection, MergedIntersection>();
            var intersectionSideMap = new Dictionary<MergedIntersection, Game.Street[]>();
            var intersectionQuadMap = new Dictionary<MergedIntersection, NavQuad>();

            foreach (var mergedIntersection in this.game.GetList<MergedIntersection>())
            {
                var streets = new Game.Street[4];

                foreach (var street in mergedIntersection.Streets)
                {
                    var thisNode = mergedIntersection
                        .Intersections.First(i => i == street.Node1 || i == street.Node2);
                    var otherNode = street.OtherNode(thisNode);

                    var diff = otherNode.Position - thisNode.Position;

                    if (diff.X == 0.U())
                    {
                        streets[diff.Y > 0.U() ? Block.Side.North : Block.Side.South] = street;
                    }
                    else if (diff.Y == 0.U())
                    {
                        streets[diff.X > 0.U() ? Block.Side.East : Block.Side.West] = street;
                    }
                    else
                    {
                        throw new Exception("");
                    }
                        
                }

                var cornerSW = getIntersectionCorner(streets, Block.Corner.SouthWest);
                var cornerSE = getIntersectionCorner(streets, Block.Corner.SouthEast);
                var cornerNW = getIntersectionCorner(streets, Block.Corner.NorthWest);
                var cornerNE = getIntersectionCorner(streets, Block.Corner.NorthEast);

                var navQuad = new NavQuad(
                    cornerSW, cornerSE, cornerNW, cornerNE
                );

                navMesh.Add(navQuad);

                intersectionSideMap.Add(mergedIntersection, streets);
                intersectionQuadMap.Add(mergedIntersection, navQuad);

                foreach (var intersection in mergedIntersection.Intersections)
                {
                    intersectionMap.Add(intersection, mergedIntersection);
                }
            }

            foreach (var mergedIntersection in this.game.GetList<MergedIntersection>())
            {
                var intersectionQuad = intersectionQuadMap[mergedIntersection];

                foreach (var street in mergedIntersection.Streets
                         .Where(s => mergedIntersection.Intersections.Contains(s.Node1)))
                {
                    var side = (Block.Side)Array.IndexOf(intersectionSideMap[mergedIntersection], street);

                    var quad1 = intersectionQuad;
                    var quad2 = intersectionQuadMap[intersectionMap[street.Node2]];

                    if (side == Block.Side.East || side == Block.Side.West)
                    {
                        if (side == Block.Side.East)
                        {
                            Do.Swap(ref quad1, ref quad2);
                        }

                        var navQuad = new NavQuad(quad2.SE, quad1.SW, quad2.NE, quad1.NW);

                        navMesh.Add(navQuad);
                    }
                    else if (side == Block.Side.South || side == Block.Side.North)
                    {
                        if (side == Block.Side.South)
                        {
                            Do.Swap(ref quad1, ref quad2);
                        }

                        var navQuad = new NavQuad(quad1.NW, quad1.NE, quad2.SW, quad2.SE);

                        navMesh.Add(navQuad);
                    }
                }
            }
        }

        private static Position2 getIntersectionCorner(Game.Street[] streets, Block.Corner corner)
        {
            var xStreet = streets[corner.Y] ?? streets[corner.Y.Opposite];
            var yStreet = streets[corner.X] ?? streets[corner.X.Opposite];

            return new Position2(
                xStreet.Node1.Position.X + xStreet.Width * (corner.X == Block.Side.East ? 1 : -1),
                yStreet.Node1.Position.Y + yStreet.Width * (corner.Y == Block.Side.North ? 1 : -1)
            );
        }
   }
}