using System.Linq;
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

            foreach (var mergedIntersection in this.game.GetList<MergedIntersection>())
            {
                var rect = mergedIntersection.GetRectangle();

                var navQuad = new NavQuad(
                    new Position2(rect.TopRight),
                    new Position2(rect.TopLeft),
                    new Position2(rect.BottomRight),
                    new Position2(rect.BottomLeft)
                );

                navMesh.Add(navQuad);
            }
        }
   }
}