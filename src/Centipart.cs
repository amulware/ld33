using amulware.Graphics;
using Bearded.Utilities.SpaceTime;

namespace Game
{
    class Centipart
    {
        private Position2 position;

        public Position2 Position
        {
            get { return this.position; }
        }

        public void SetPosition(Position2 position)
        {
            this.position = position;
        }

        public void Draw()
        {
            var geo = GeometryManager.Instance.Primitives;

            geo.Color = Color.IndianRed;
            geo.DrawCircle(this.position.Vector, 0.9f);

        }
    }
}