using amulware.Graphics;
using Bearded.Utilities;
using Bearded.Utilities.Linq;
using Bearded.Utilities.SpaceTime;
using Centipede.Rendering;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Centipede.Game
{
    internal class Civilian : GameObject
    {
        private readonly Sprite2DGeometry sprite;

        private Position2 position;
        private Street currentStreet;
        private Intersection goalIntersection;
        private Position2 goalPoint;

        public Civilian(GameState game)
            : base(game)
        {
            this.sprite = (Sprite2DGeometry)GeometryManager.Instance.GetSprite("bloob").Geometry;

            this.currentStreet = this.game.GetList<Street>().RandomElement();
            var streetVector = (this.currentStreet.Node1.Position - this.currentStreet.Node2.Position)
                .Vector.Normalized();
            this.position = this.currentStreet.Node1.Position +
                            (this.currentStreet.Node2.Position - this.currentStreet.Node1.Position) *
                            StaticRandom.Float()
                            + new Difference2(streetVector)
                             * (this.currentStreet.Width.NumericValue * StaticRandom.Float(-1, 1));
            this.goalIntersection = StaticRandom.Bool() ? this.currentStreet.Node1 : this.currentStreet.Node2;
            this.updateGoalPoint();
        }

        public override void Update(TimeSpan elapsedTime)
        {
            var maxMoveThisFrame = 2.U() * (float)elapsedTime.NumericValue;

            var differenceToGoal = this.goalPoint - this.position;
            var distanceToGoal = differenceToGoal.Length;

            if (distanceToGoal.Units < maxMoveThisFrame)
            {
                this.position = this.goalPoint;
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
                this.position += new Difference2(differenceToGoal.Vector / distanceToGoal.NumericValue) *
                                 maxMoveThisFrame.NumericValue;
            }

        }

        private void updateGoalPoint()
        {
            var streetVector = (this.currentStreet.Node1.Position - this.currentStreet.Node2.Position)
                .Vector.Normalized();
            this.goalPoint = this.goalIntersection.Position
                             + new Difference2(streetVector)
                             * (this.currentStreet.Width.NumericValue * StaticRandom.Float(-1, 1));
        }

        public override void Draw()
        {
            this.sprite.DrawSprite(this.position.Vector, 0, 1);
        }
    }
}