using amulware.Graphics;
using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;
using Centipede.Rendering;

namespace Centipede.Game
{
    class CollidingParticle : GameObject, IPositionable
    {
        private Data data;

        public Position2 Position { get { return this.data.Position; } }

        public CollidingParticle(GameState game,
            Position2 position, Velocity2 velocity,
            Unit z, Speed vz,
            bool collideWithProjectileColliders)
            : base(game)
        {
            this.data = new Data(position, velocity, z, vz, collideWithProjectileColliders);
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
            private Unit z;
            private Velocity2 velocity;
            private Speed vz;
            private readonly bool collideWithProjectileColliders;

            public Data(Position2 p, Velocity2 v, Unit z, Speed vz, bool collideWithProjectileColliders)
            {
                this.position = p;
                this.velocity = v;
                this.z = z;
                this.vz = vz;
                this.collideWithProjectileColliders = collideWithProjectileColliders;
            }

            public Position2 Position { get { return this.position; } }
            public Velocity2 Velocity { get { return this.velocity; } }

            public Unit Z { get { return this.z; } }
            public Speed VZ { get { return this.vz; } }

            public HitResult? Update(GameState game, TimeSpan time)
            {
                var vDelta = this.velocity * time;

                var hitResult = new Ray(this.position, vDelta)
                    .Shoot(game, true, this.collideWithProjectileColliders);

                var f = hitResult.HasValue ? hitResult.Value.RayFactor : 1;

                var zDelta = this.vz * time;

                var z = this.z + zDelta;
                if (z < 0.U())
                {
                    f = this.z / zDelta;
                    z = 0.U();
                    hitResult = new HitResult(this.position + vDelta * f, Direction2.Zero, f, false);
                }

                this.position += vDelta * f;
                this.z = z;

                this.vz += game.Gravity * time;

                return hitResult;
            }

            public Data UpdateTo(GameState game, TimeSpan time, out HitResult? hitResult)
            {
                hitResult = this.Update(game, time);
                return this;
            }

            public void Draw(Sprite sprite, float size)
            {
                sprite.Draw(this.position.NumericValue.WithZ(this.z.NumericValue), 0, size);
            }
        }
    }
}