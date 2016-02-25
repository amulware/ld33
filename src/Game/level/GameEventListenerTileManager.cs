using System.Collections.Generic;
using Bearded.Utilities.Tilemaps.Rectangular;

namespace Centipede.Game
{
    class GameEventListenerTileManager : SinglePointTileContainmantManager
    {
        private readonly LinkedListNode<IGameEventListener> node = new LinkedListNode<IGameEventListener>(null);

        public GameEventListenerTileManager(GameState game, IGameEventListener listener)
            : base(game, listener)
        {
            this.node.Value = listener;
        }

        protected override void addToTile(Tile<TileInfo> tile)
        {
            tile.Value.EventListeners.AddLast(this.node);
        }

        protected override void removeFromTile(Tile<TileInfo> tile)
        {
            tile.Value.EventListeners.Remove(this.node);
        }
    }
}