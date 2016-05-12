using System.Linq;
using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;
using Centipede.Rendering;
using OpenTK;

namespace Centipede.Game
{
    class PlayerView :GameObject
    {
        private readonly IPositionable parent;

        private Building currentlyInside;

        public PlayerView(GameState game, IPositionable parent)
            : base(game)
        {
            this.parent = parent;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            this.updateCurrentBuilding();
        }

        private void updateCurrentBuilding()
        {
            var p = this.parent.Position;

            var tile = this.game.Level[p];

            this.setCurrentBuilding(!tile.IsValid ? null : tile.Value.Buildings.FirstOrDefault(b => b.IsInside(p)));
        }

        private void setCurrentBuilding(Building building)
        {
            if (building == this.currentlyInside)
                return;

            if (this.currentlyInside != null)
                this.currentlyInside.RevealInside = false;

            if (building != null)
                building.RevealInside = true;

            this.currentlyInside = building;
        }


        public override void Draw()
        {
            var xy = this.parent.Position.NumericValue;

            SurfaceManager.Instance.ModelviewMatrix.Matrix
                = Matrix4.LookAt(
                    xy.WithZ(50), xy.WithZ(), new Vector3(0, 1, 0)
                );
        }
    }
}