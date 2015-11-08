﻿using amulware.Graphics;
using Bearded.Utilities.SpaceTime;
using Centipede.Rendering;

namespace Centipede.Game
{
    struct Ray
    {
        private readonly Position2 start;
        private readonly Difference2 direction;

        public Ray(Position2 start, Difference2 direction)
        {
            this.start = start;
            this.direction = direction;
        }

        public Position2 Start { get { return this.start; } }
        public Difference2 Direction { get { return this.direction; } }

        public HitResult? Shoot(GameState game)
        {
            HitResult? result = null;

            var bestF = 1f;

            foreach (var building in game.Buildings)
            {
                var r = building.TryHit(this);
                if (r.HasValue && r.Value.RayFactor < bestF)
                {
                    bestF = r.Value.RayFactor;
                    result = r.Value;
                }
            }

            var geo = GeometryManager.Instance.Primitives;
            geo.LineWidth = 0.05f;

            if (result.HasValue)
            {
                var p = result.Value.Point.Vector;
                geo.Color = Color.Green;
                geo.DrawLine(this.Start.Vector, p);
                geo.DrawCircle(p, 0.2f);
            }
            else
            {
                geo.Color = Color.Red;
                geo.DrawLine(this.Start.Vector, (this.Start + this.Direction).Vector);
            }

            return result;
        }
    }
}