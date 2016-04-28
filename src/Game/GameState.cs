using System;
using System.Collections.Generic;
using amulware.Graphics;
using Bearded.Utilities.Collections;
using Bearded.Utilities.SpaceTime;
using Centipede.Game.Generation;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Centipede.Game
{
    sealed class GameState
    {
        private readonly DeletableObjectList<GameObject> gameObjects = new DeletableObjectList<GameObject>();
        private readonly Dictionary<Type, object> lists = new Dictionary<Type, object>();

        public Instant Time { get; private set; } = Instant.Zero;

        public Level Level { get; }

        public Acceleration Gravity => new Acceleration(-10);


        public GameState()
        {
            var width = 200;
            var height = 200;

            new StreetGenerator().Generate(this, width, height);

            this.Level = new Level(this, width, height);
            
            var pede = new Centipede(this);
            
            new PlayerView(this, pede);

            for (int i = 0; i < 1000; i++)
            {
                new Civilian(this);
            }
        }

        public void Add(GameObject gameObject)
        {
            this.gameObjects.Add(gameObject);
        }

        public void ListAs<T>(T obj)
            where T : class, IDeletable
        {
            this.GetList<T>().Add(obj);
        }

        public DeletableObjectList<T> GetList<T>()
            where T : class, IDeletable
        {
            object list;
            if (this.lists.TryGetValue(typeof(T), out list))
                return (DeletableObjectList<T>)list;

            var l = new DeletableObjectList<T>();
            this.lists.Add(typeof(T), l);
            return l;
        }

        public void Update(UpdateEventArgs args)
        {
            var elapsedTime = new TimeSpan(args.ElapsedTimeInS);

            this.Time += elapsedTime;


            foreach (var gameObject in this.gameObjects)
            {
                gameObject.Update(elapsedTime);
            }
        }

        public void Render()
        {
            //this.level.DebugDraw();

            foreach (var gameObject in this.gameObjects)
            {
                gameObject.Draw();
            }
        }
    }
}