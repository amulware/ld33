using System;
using amulware.Graphics;
using Bearded.Utilities.SpaceTime;
using Centipede.Rendering;
using OpenTK;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

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

        public Intersection Node1 { get { return this.node1; } }
        public Intersection Node2 { get { return this.node2; } }

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

        public Intersection OtherNode(Intersection node)
        {
#if DEBUG
            if(node != this.node1 && node != this.node2)
                throw new Exception("Given node is invalid");
#endif
            return node == this.node1 ? this.node2 : this.node1;
        }
    }
}