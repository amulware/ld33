using Bearded.Utilities.Tilemaps.Rectangular;

namespace Centipede.Game
{
    abstract class SinglePointTileContainmantManager
    {
        private readonly GameState game;
        private readonly IPositionable subject;

        private Tile<TileInfo> currentTile;

        protected SinglePointTileContainmantManager(GameState game, IPositionable subject)
        {
            this.game = game;
            this.subject = subject;

            this.Update();
        }

        public void Update()
        {
            var p = this.subject.Position;

            var tile = this.game.Level[p];

            if (tile == this.currentTile)
                return;

            if (this.currentTile.IsValid)
                this.removeFromTile(this.currentTile);

            if (tile.IsValid)
                this.addToTile(tile);

            this.currentTile = tile;
        }

        protected abstract void addToTile(Tile<TileInfo> tile);
        protected abstract void removeFromTile(Tile<TileInfo> tile);
    }
}