using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;
using OpenTK;

namespace Centipede.Game
{
    interface IProjectileCollider
    {
        Position2 Center { get; }
        Unit Radius { get; }
    }

    static class ProjectileCollider
    {
        public static HitResult? TryHit(this IProjectileCollider collider, Ray ray)
        {
            var start = ray.Start.NumericValue;
            var dir = ray.Direction.NumericValue;
            var center = collider.Center.NumericValue;
            var radius = collider.Radius.NumericValue;

            var a = start.X - center.X;
            var b = start.Y - center.Y;
            var r2 = radius * radius;

            var c2 = dir.X * dir.X;
            var d2 = dir.Y * dir.Y;
            var cd = dir.X * dir.Y;

            var s = (r2 - a * a) * d2
                    + (r2 - b * b) * c2
                    + 2 * a * b * cd;

            // if s is less than 0, the solutions for f are imaginary
            // and the ray's line does not intersect the circle
            if (s >= 0)
            {
                var f = (Mathf.Sqrt(s) + a * dir.X + b * dir.Y) / -(c2 + d2);

                if (f <= 1)
                {
                    var isInside = a * a + b * b < r2;
                    if (f >= 0 || (isInside && !float.IsNegativeInfinity(f)))
                    {
                        // TODO: currently returns negative f if ray starts inside
                        // consider treating this case differently?
                        // do we ever care about 'exit hits'?
                        // probably want to split entry/exit hits
                        // how to handle moving targets?
                        return new HitResult(
                            new Position2(start + dir * f),
                            Direction2.Of(new Vector2(a, b)),
                            f,
                            isInside
                            );
                    }
                }
            }

            return null;
        }
    }
}