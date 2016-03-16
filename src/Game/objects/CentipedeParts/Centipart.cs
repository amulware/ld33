using amulware.Graphics;
using Bearded.Utilities.SpaceTime;
using Centipede.Rendering;

namespace Centipede.Game.CentipedeParts
{
    class Centipart : IProjectileCollider
    {
        private readonly ProjectileColliderTileManager projectileCollider;

        public Centipart(GameState game)
        {
            this.projectileCollider = new ProjectileColliderTileManager(game, this);
        }

        public Position2 Position { get; protected set; }

        Unit IProjectileCollider.Radius => 0.9.U();

        public void SetPosition(Position2 position)
        {
            this.Position = position;
            this.update();
        }

        protected void update()
        {
            this.projectileCollider.Update();
        }

        public void Delete()
        {
            this.projectileCollider.Cleanup();
        }

        public virtual void Draw()
        {
            var geo = GeometryManager.Instance.Primitives;

            geo.Color = Color.IndianRed;
            geo.DrawCircle(this.Position.NumericValue, 0.9f);
        }

    }
}