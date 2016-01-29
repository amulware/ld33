using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Centipede.Game
{
    class TileInfo
    {
        private readonly List<Building> buildings = new List<Building>();
        public ReadOnlyCollection<Building> Buildings { get; private set; }

        public TileInfo()
        {
            this.Buildings = this.buildings.AsReadOnly();
        }

        public void AddBuilding(Building building)
        {
            this.buildings.Add(building);
        }
    }
}