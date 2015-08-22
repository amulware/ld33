using Bearded.Utilities.SpaceTime;

namespace Game
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

            return result;
        }
    }
}