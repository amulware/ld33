using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Centipede.Game
{
    class TileInfo
    {
        private readonly List<Building> buildings = new List<Building>();


        private readonly LinkedList<IProjectileCollider> projectileColliders =
            new LinkedList<IProjectileCollider>();

        public TileInfo()
        {
            this.Buildings = this.buildings.AsReadOnly();
        }

        public ReadOnlyCollection<Building> Buildings { get; private set; }
        public LinkedList<IProjectileCollider> ProjectileColliders { get { return this.projectileColliders; } }

        public void AddBuilding(Building building)
        {
            this.buildings.Add(building);
        }
    }
}