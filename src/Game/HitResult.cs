using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;

namespace Centipede.Game
{
    struct HitResult
    {
        public HitResult(Position2 point, Direction2 normal, float rayFactor, bool fromInside)
        {
            this.Point = point;
            this.Normal = normal;
            this.RayFactor = rayFactor;
            this.FromInside = fromInside;
        }

        public Position2 Point { get; }
        public Direction2 Normal { get; }
        public float RayFactor { get; }
        public bool FromInside { get; }
    }
}