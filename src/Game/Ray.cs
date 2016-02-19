using amulware.Graphics;
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

        public HitResult? Shoot(GameState game, bool hitBuildings = true, bool hitProjectileColliders = false)
        {
            HitResult? result = null;

            var bestF = 1f;

            foreach (var tile in game.Level.CastRay(this))
            {
                if (!tile.IsValid)
                    continue;

                var info = tile.Value;

                if (hitBuildings)
                {
                    foreach (var building in info.Buildings)
                    {
                        var r = building.TryHit(this);
                        selectBetterResult(ref result, ref bestF, r);
                    }
                }

                if (hitProjectileColliders)
                {
                    foreach (var collider in info.ProjectileColliders)
                    {
                        var r = collider.TryHit(this);
                        selectBetterResult(ref result, ref bestF, r);
                    }
                }
            }

            return result;
        }

        public HitResult? Shoot(GameState game, bool hitBuildings, bool hitProjectileColliders, bool debugDraw)
        {
            var result = this.Shoot(game, hitBuildings, hitProjectileColliders);

            if (debugDraw)
                this.drawDebug(result);

            return result;
        }

        private static void selectBetterResult(ref HitResult? currentBest, ref float currentBestF, HitResult? candidate)
        {
            if (candidate.HasValue)
            {
                var f = candidate.Value.RayFactor;
                if (f < currentBestF)
                {
                    currentBest = candidate;
                    currentBestF = f;
                }
            }
        }

        private void drawDebug(HitResult? result)
        {
            var geo = GeometryManager.Instance.PrimitivesOverlay;
            geo.LineWidth = 0.05f;

            if (result.HasValue)
            {
                var p = result.Value.Point.NumericValue;
                geo.Color = Color.Green;
                geo.DrawLine(this.Start.NumericValue, p);
                geo.DrawCircle(p, 0.2f);
            }
            else
            {
                geo.Color = Color.Red;
                geo.DrawLine(this.Start.NumericValue, (this.Start + this.Direction).NumericValue);
            }
        }
    }
}