using System;
using amulware.Graphics;
using Bearded.Utilities.SpaceTime;
using Centipede.Game.AI;
using Centipede.Rendering;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Centipede.Game
{
    class Civilian : GameObject, IGameEventListener
    {
        private readonly Sprite2DGeometry sprite;
        private readonly GameEventListenerTileManager eventListenerTileManager;
        private readonly CivilianController controller;

        private Position2 goalPoint;

        readonly bool initialised;


        public Civilian(GameState game)
            : base(game)
        {
            this.sprite = (Sprite2DGeometry)GeometryManager.Instance.GetSprite("bloob").Geometry;

            this.controller = new CivilianController(game, this);
            this.eventListenerTileManager = new GameEventListenerTileManager(game, this);

            this.initialised = true;
        }

        public Position2 Position { get; private set; }

        public override void Update(TimeSpan elapsedTime)
        {
            this.goalPoint = this.controller.Control();

            var maxMoveThisFrame = new Speed(2) * elapsedTime;

            var differenceToGoal = this.goalPoint - this.Position;
            var distanceToGoal = differenceToGoal.Length;

            if (distanceToGoal < maxMoveThisFrame)
            {
                this.Position = this.goalPoint;
            }
            else
            {
                this.Position += differenceToGoal / distanceToGoal * maxMoveThisFrame;
            }

            this.eventListenerTileManager.Update();
        }

        public void SetPosition(Position2 position)
        {
            this.ensureInitilizing();
            this.Position = position;
        }

        private void ensureInitilizing()
        {
#if DEBUG
            if (this.initialised)
                throw new Exception();
#endif
        }

        public bool TryPerceive(IGameEvent e)
        {
            if (!e.CanBePerceivedAt(this.Position))
                return false;

            return true;
        }

        public override void Draw()
        {
            this.sprite.DrawSprite(this.Position.NumericValue, 0, 1);

//            var geo = GeometryManager.Instance.Primitives;
//            geo.Color = Color.Green;
//            geo.DrawRectangle(this.goalPoint.NumericValue, new OpenTK.Vector2(0.5f, 0.5f));
//            geo.DrawLine(this.Position.NumericValue, this.goalPoint.NumericValue);
        }

    }
}