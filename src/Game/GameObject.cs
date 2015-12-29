using System;
using Bearded.Utilities.Collections;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Centipede.Game
{
    abstract class GameObject : IDeletable
    {
        protected readonly GameState game;

        public bool Deleted { get; private set; }

        protected GameObject(GameState game)
        {
            this.game = game;

            game.Add(this);
        }

        protected void listAs<T>()
            where T : class, IDeletable
        {
            var asT = this as T;
#if DEBUG
            if (asT == null)
                throw new Exception("Cannot list as incompatible type");
#endif
            this.game.ListAs(asT);
        }

        public abstract void Update(TimeSpan elapsedTime);

        public abstract void Draw();

        protected virtual void onDelete()
        {
            
        }

        public void Delete()
        {
            this.onDelete();
            this.Deleted = true;
        }

    }
}