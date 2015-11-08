using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;

namespace Centipede.Game
{
    struct HitResult
    {
        private readonly Position2 point;
        private readonly Direction2 normal;
        private readonly float rayFactor;
        private readonly bool fromInside;

        public HitResult(Position2 point, Direction2 normal, float rayFactor, bool fromInside)
        {
            this.point = point;
            this.normal = normal;
            this.rayFactor = rayFactor;
            this.fromInside = fromInside;
        }

        public Position2 Point { get { return this.point; } }
        public Direction2 Normal { get { return this.normal; } }
        public float RayFactor { get { return this.rayFactor; } }
        public bool FromInside { get { return this.fromInside; } }
    }
}