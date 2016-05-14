using amulware.Graphics;
using Bearded.Utilities.Linq;
using Bearded.Utilities.SpaceTime;
using Centipede.Rendering;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Centipede.Game
{
    class Civilian : GameObject, IGameEventListener
    {
        private readonly Sprite2DGeometry sprite;

        private NavQuad currentQuad;
        private NavLink goalLink;
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
            this.currentQuad = this.game.Get<NavMesh>().NavQuads.RandomElement();
            this.Position = currentQuad.RandomPointInside();
            this.goalLink = this.currentQuad.Links.RandomElement();
        }

        public override void Update(TimeSpan elapsedTime)
        {
            var maxMoveThisFrame = new Speed(2) * elapsedTime;

            var differenceToGoal = this.goalPoint - this.Position;
            var distanceToGoal = differenceToGoal.Length;

            if (distanceToGoal < maxMoveThisFrame)
            {
                this.Position = this.goalPoint;
                if (this.goalLink.To.Links.Count > 1)
                {
                    var oldQuad = this.currentQuad;
                    this.currentQuad = this.goalLink.To;
                    while(true)
                    {
                        this.goalLink = this.currentQuad.Links.RandomElement();

                        if (oldQuad != this.goalLink.To)
                            break;
                    }
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
            this.goalPoint = this.goalLink.RandomPoint();
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