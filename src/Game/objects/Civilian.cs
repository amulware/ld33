using amulware.Graphics;
using Bearded.Utilities;
using Bearded.Utilities.Linq;
using Bearded.Utilities.SpaceTime;
using Centipede.Rendering;
using OpenTK;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Centipede.Game
{
    internal class Civilian : GameObject, IGameEventListener
    {
        private readonly Sprite2DGeometry sprite;

        private Street currentStreet;
        private Intersection goalIntersection;
        private Position2 goalPoint;
        private readonly GameEventListenerTileManager eventListenerTileManager;

        public Civilian(GameState game)
            : base(game)
        {
            this.sprite = (Sprite2DGeometry)GeometryManager.Instance.GetSprite("bloob").Geometry;

            this.initializePosition();
            this.updateGoalPoint();

            this.eventListenerTileManager = new GameEventListenerTileManager(game, this);
        }

        public Position2 Position { get; private set; }

        private void initializePosition()
        {
            this.currentStreet = this.game.GetList<Street>().RandomElement();
            var streetVector = (this.currentStreet.Node1.Position - this.currentStreet.Node2.Position)
                .NumericValue.PerpendicularLeft.Normalized();
            this.Position = this.currentStreet.Node1.Position
                .LerpTo(this.currentStreet.Node2.Position, StaticRandom.Float())
                            + streetVector * (this.currentStreet.Width * StaticRandom.Float(-1, 1));
            this.goalIntersection = StaticRandom.Bool() ? this.currentStreet.Node1 : this.currentStreet.Node2;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            var maxMoveThisFrame = new Speed(2) * elapsedTime;

            var differenceToGoal = this.goalPoint - this.Position;
            var distanceToGoal = differenceToGoal.Length;

            if (distanceToGoal < maxMoveThisFrame)
            {
                this.Position = this.goalPoint;
                if (this.goalIntersection.Streets.Count > 1)
                {
                    var oldStreet = this.currentStreet;
                    while (oldStreet == this.currentStreet)
                    {
                        this.currentStreet = this.goalIntersection.Streets.RandomElement();
                    }
                    this.goalIntersection = this.currentStreet.OtherNode(this.goalIntersection);
                    this.updateGoalPoint();
                }
            }
            else
            {
                this.Position += differenceToGoal / distanceToGoal * maxMoveThisFrame;
            }

            this.eventListenerTileManager.Update();
        }

        private void updateGoalPoint()
        {
            var streetVector = (this.currentStreet.Node1.Position - this.currentStreet.Node2.Position)
                .NumericValue.PerpendicularLeft.Normalized();
            this.goalPoint = this.goalIntersection.Position
                             + streetVector * (this.currentStreet.Width * StaticRandom.Float(-1, 1));
        }

        public bool TryPerceive(IGameEvent @event)
        {
            throw new System.NotImplementedException();
        }

        public override void Draw()
        {
            this.sprite.DrawSprite(this.Position.NumericValue, 0, 1);

            //var geo = GeometryManager.Instance.Primitives;
            //geo.Color = Color.Green;
            //geo.DrawRectangle(this.goalPoint.Vector, new Vector2(0.5f, 0.5f));
            //geo.DrawLine(this.position.Vector, this.goalPoint.Vector);
        }

    }
}