using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Centipede.Game
{
    class TileInfo
    {
        private readonly List<Building> buildings = new List<Building>();
        private readonly LinkedList<IProjectileCollider> projectileColliders =
            new LinkedList<IProjectileCollider>();
        private readonly LinkedList<IGameEventListener> eventListeners =
            new LinkedList<IGameEventListener>(); 

        public TileInfo()
        {
            this.Buildings = this.buildings.AsReadOnly();
        }

        public ReadOnlyCollection<Building> Buildings { get; private set; }
        public LinkedList<IProjectileCollider> ProjectileColliders { get { return this.projectileColliders; } }
        public LinkedList<IGameEventListener> EventListeners { get { return this.eventListeners; } }

        public void AddBuilding(Building building)
        {
            this.buildings.Add(building);
        }
    }
}