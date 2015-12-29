using amulware.Graphics;
using Bearded.Utilities.SpaceTime;
using Centipede.Rendering;

namespace Centipede.Game
{
    class Street : GameObject
    {
        private readonly Intersection node1;
        private readonly Intersection node2;
        private readonly Radius width;

        public Street(GameState game, Intersection node1, Intersection node2, Radius width)
            : base(game)
        {
            this.node1 = node1;
            this.node2 = node2;
            this.width = width;

            node1.AddStreet(this);
            node2.AddStreet(this);

            this.listAs<Street>();
        }

        public override void Update(TimeSpan elapsedTime)
        {

        }

        public override void Draw()
        {
            var geo = GeometryManager.Instance.Primitives;

            geo.Color = Color.DarkGray;
            geo.LineWidth = this.width.NumericValue - 1;
            geo.DrawLine(this.node1.Position.Vector, this.node2.Position.Vector);
        }
    }
}