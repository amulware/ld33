using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Centipede.Game
{
    class TileInfo
    {
        private readonly List<Building> buildings = new List<Building>();

        public LinkedList<IProjectileCollider> ProjectileColliders { get; }
            = new LinkedList<IProjectileCollider>();
        public LinkedList<IGameEventListener> EventListeners { get; }
            = new LinkedList<IGameEventListener>();

        public TileInfo()
        {
            this.Buildings = this.buildings.AsReadOnly();
        }

        public ReadOnlyCollection<Building> Buildings { get; private set; }

        public void AddBuilding(Building building)
        {
            this.buildings.Add(building);
        }
    }
}