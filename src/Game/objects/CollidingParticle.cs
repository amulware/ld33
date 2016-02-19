using amulware.Graphics;
using Bearded.Utilities.SpaceTime;
using Centipede.Rendering;

namespace Centipede.Game
{
    class CollidingParticle : GameObject
    {
        private Data data;

        public CollidingParticle(GameState game, Position2 position, Velocity2 velocity, bool collideWithProjectileColliders)
            : base(game)
        {
            this.data = new Data(position, velocity, collideWithProjectileColliders);
        }

        public override void Update(TimeSpan elapsedTime)
        {
            var hitResult = this.data.Update(this.game, elapsedTime);

            if (hitResult.HasValue)
                this.onHit(hitResult.Value);
        }

        protected virtual void onHit(HitResult hitResult)
        {
            this.Delete();
        }

        public override void Draw()
        {
            this.data.Draw(GeometryManager.Instance.GetSprite("bloob"), 0.2f);
        }
        
        public struct Data
        {
            private Position2 position;
            private Velocity2 velocity;
            private readonly bool collideWithProjectileColliders;

            public Data(Position2 p, Velocity2 v, bool collideWithProjectileColliders)
            {
                this.position = p;
                this.velocity = v;
                this.collideWithProjectileColliders = collideWithProjectileColliders;
            }

            public Position2 Position { get { return this.position; } }
            public Velocity2 Velocity { get { return this.velocity; } }

            public HitResult? Update(GameState game, TimeSpan time)
            {
                var vDelta = this.velocity * time;

                var hitResult = new Ray(this.position, vDelta)
                    .Shoot(game, true, this.collideWithProjectileColliders);

                if (hitResult.HasValue)
                    vDelta *= hitResult.Value.RayFactor;

                this.position += vDelta;

                return hitResult;
            }

            public void Draw(Sprite sprite, float size)
            {
                sprite.Draw(this.position.NumericValue, 0, size);
            }
        }

    }
}