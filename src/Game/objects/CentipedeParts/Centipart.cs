using amulware.Graphics;
using Bearded.Utilities.SpaceTime;
using Centipede.Rendering;

namespace Centipede.Game.CentipedeParts
{
    class Centipart : IProjectileCollider
    {
        protected Position2 position;

        private readonly ProjectileColliderTileManager projectileCollider;

        public Centipart(GameState game)
        {
            this.projectileCollider = new ProjectileColliderTileManager(game, this);
        }

        public Position2 Position
        {
            get { return this.position; }
        }
        Position2 IProjectileCollider.Center { get { return this.position; } }
        Unit IProjectileCollider.Radius { get { return 0.9.U(); } }

        public void SetPosition(Position2 position)
        {
            this.position = position;
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
            geo.DrawCircle(this.position.NumericValue, 0.9f);

        }

    }
}