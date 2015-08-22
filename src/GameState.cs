using amulware.Graphics;
using Bearded.Utilities.Collections;
using Bearded.Utilities.SpaceTime;

namespace Game
{
    sealed class GameState
    {
        private readonly DeletableObjectList<GameObject> gameObjects = new DeletableObjectList<GameObject>();
        private Instant time = Instant.Zero;

        public Instant Time { get { return this.time; } }

        public GameState()
        {

            for (int y = -10; y < 10; y++)
            {
                for (int x = -10; x < 10; x++)
                {
                    new Building(this, new Position2(x * 10, y * 10), new Difference2(6, 6));
                }
            }

            new Centipede(this);
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
    }
}