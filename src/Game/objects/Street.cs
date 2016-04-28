using System;
using amulware.Graphics;
using Bearded.Utilities.SpaceTime;
using Centipede.Rendering;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Centipede.Game
{
    class Street : GameObject
    {
        public Street(GameState game, Intersection node1, Intersection node2, Unit width)
            : base(game)
        {
            this.Node1 = node1;
            this.Node2 = node2;
            this.Width = width;

            node1.AddStreet(this);
            node2.AddStreet(this);

            this.listAs<Street>();
        }

        public Intersection Node1 { get; }
        public Intersection Node2 { get; }
        public Unit Width { get; }

        public override void Update(TimeSpan elapsedTime)
        {

        }

        public override void Draw()
        {
            var geo = GeometryManager.Instance.Primitives;

            geo.Color = Color.DarkGray;
            geo.LineWidth = this.Width.NumericValue;// - 1;
            geo.DrawLine(this.Node1.Position.NumericValue, this.Node2.Position.NumericValue);
        }

        public Intersection OtherNode(Intersection node)
        {
#if DEBUG
            if(node != this.Node1 && node != this.Node2)
                throw new Exception("Given node is invalid");
#endif
            return node == this.Node1 ? this.Node2 : this.Node1;
        }

        protected override void onDelete()
        {
            this.Node1.RemoveStreet(this);
            this.Node2.RemoveStreet(this);
        }
    }
}