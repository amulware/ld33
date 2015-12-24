using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;
using Centipede.Rendering;
using OpenTK;

namespace Centipede.Game
{
    class PlayerView :GameObject
    {
        private readonly IPositionable parent;

        public PlayerView(GameState game, IPositionable parent)
            : base(game)
        {
            this.parent = parent;
        }

        public override void Update(TimeSpan elapsedTime)
        {

        }

        public override void Draw()
        {
            var xy = this.parent.Position.Vector;

            SurfaceManager.Instance.ModelviewMatrix.Matrix
                = Matrix4.LookAt(
                    xy.WithZ(500), xy.WithZ(), new Vector3(0, 1, 0)
                );
        }
    }

    interface IPositionable
    {
        Position2 Position { get; }
    }
}