using amulware.Graphics;
using Bearded.Utilities.SpaceTime;
using Centipede.Rendering;

namespace Centipede.Game.CentipedeParts
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
            geo.DrawCircle(this.position.NumericValue, 0.9f);

        }
    }
}