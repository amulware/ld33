
using System.Collections.Generic;
using System.Linq;
using amulware.Graphics;
using Bearded.Utilities.SpaceTime;
using Centipede.Rendering;

namespace Centipede.Game
{
    class MergedIntersection : GameObject
    {
        private readonly List<Intersection> intersections;

        public MergedIntersection(GameState game, params Intersection[] intersections)
            : base(game)
        {
            this.intersections = intersections.ToList();
        }

        public override void Draw()
        {
            var geo = GeometryManager.Instance.Primitives;

            geo.Color = Color.Red * 0.5f;


        }
                              

        public override void Update(TimeSpan elapsedTime)
        {
        }
    }
}

