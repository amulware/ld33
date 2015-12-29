using amulware.Graphics;
using Bearded.Utilities.Collections;
using Bearded.Utilities.SpaceTime;
using Centipede.Game.Generation;

namespace Centipede.Game
{
    sealed class GameState
    {
        private readonly DeletableObjectList<GameObject> gameObjects = new DeletableObjectList<GameObject>();
        private DeletableObjectList<Building> buildings = new DeletableObjectList<Building>();

        private Instant time = Instant.Zero;

        public Instant Time { get { return this.time; } }

        public DeletableObjectList<Building> Buildings { get { return this.buildings; } }


        public GameState()
        {
            new StreetGenerator().Generate(this);
            
            var pede = new Centipede(this);
            
            new PlayerView(this, pede);

        }

        public void Add(GameObject gameObject)
        {
            this.gameObjects.Add(gameObject);
        }

        public void Update(UpdateEventArgs args)
        {
            var elapsedTime = new TimeSpan(args.ElapsedTimeInS);

            this.time += elapsedTime;


            foreach (var gameObject in this.gameObjects)
            {
                gameObject.Update(elapsedTime);
            }
        }

        public void Render()
        {
            foreach (var gameObject in this.gameObjects)
            {
                gameObject.Draw();
            }
        }

        public void AddBuilding(Building building)
        {
            this.buildings.Add(building);
        }
    }
}