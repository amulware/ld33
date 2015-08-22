using amulware.Graphics;
using Bearded.Utilities.SpaceTime;

namespace Game
{
    sealed class Building : GameObject
    {
        private Position2 topLeft;
        private Difference2 size;

        public Building(GameState game, Position2 topLeft, Difference2 size)
            : base(game)
        {
            this.topLeft = topLeft;
            this.size = size;
        }

        public override void Update(TimeSpan elapsedTime)
        {
        }

        public override void Draw()
        {
            var geo = GeometryManager.Instance.Primitives;

            geo.Color = Color.Gray;
            geo.DrawRectangle(this.topLeft.Vector, this.size.Vector);
        }
    }
}