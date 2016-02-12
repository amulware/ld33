using System.Collections.Generic;
using System.Linq;
using Bearded.Utilities.SpaceTime;
using Bearded.Utilities.Tilemaps.Rectangular;

namespace Centipede.Game
{
    class ProjectileColliderTileManager
    {
        private class Tuple
        {
            public Tile<TileInfo> Tile { get; set; }
            public LinkedListNode<IProjectileCollider> Node { get; private set; }

            public Tuple(IProjectileCollider collider)
            {
                this.Node = new LinkedListNode<IProjectileCollider>(collider);
            }
        }

        private readonly GameState game;
        private readonly IProjectileCollider collider;
        private readonly List<Tuple> nodes = new List<Tuple>(4);

        private TileRectangle currentTiles = TileRectangle.Dummy;
        private int tileCount;

        public ProjectileColliderTileManager(GameState game, IProjectileCollider collider)
        {
            this.game = game;
            this.collider = collider;

            this.Update();
        }

        public void Update()
        {
            var p = this.collider.Center;
            var r = this.collider.Radius;

            var d = new Difference2(r, r);

            var tiles = this.game.Level.RectangleIntersecting(p - d, d * 2);

            if (tiles == this.currentTiles)
                return;

            this.clearTuples();

            this.tileCount = tiles.Tiles;

            this.ensureTuples();
            this.fillTiles(tiles);

            this.currentTiles = tiles;
        }

        private void clearTuples()
        {
            for (var i = 0; i < this.tileCount; i++)
            {
                var node = this.nodes[i].Node;
                node.List.Remove(node);
            }
        }

        private void ensureTuples()
        {
            for (var i = this.nodes.Count; i < this.tileCount; i++)
            {
                this.nodes.Add(new Tuple(this.collider));
            }
        }

        private void fillTiles(TileRectangle tiles)
        {
            var i = 0;
            foreach (var tile in this.game.Level.TilesIn(tiles))
            {
                var node = this.nodes[i++];
                node.Tile = tile;
                tile.Value.ProjectileColliders.AddLast(node.Node);
            }
        }

        public IEnumerable<Tile<TileInfo>> Tiles
        {
            get
            {
                for (var i = 0; i < this.tileCount; i++)
                {
                    yield return this.nodes[i].Tile;
                }
            }
        }

        public void Cleanup()
        {
            foreach (var node in this.nodes
                .Select(node => node.Node)
                .TakeWhile(node => node.List != null))
            {
                node.List.Remove(node);
            }
        }
    }
}