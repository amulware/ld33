
using System;
using Bearded.Utilities.SpaceTime;

namespace Centipede.Game.AI
{
    class CivilianController
    {
        public Civilian Civilian { get; }
        public GameState Game { get; }
        public Navigator Navigator { get; }

        IBehaviour behaviour;

        public CivilianController(GameState game, Civilian civilian)
        {
            this.Game = game;
            this.Civilian = civilian;

            this.Navigator = new Navigator(game, civilian);

            this.start<IdleWalkBehaviour>();
        }


        private void start<TBehaviour>()
            where TBehaviour : IBehaviour, new()
        {
            if (this.behaviour != null)
                this.behaviour.Stop();

            this.behaviour = new TBehaviour();
            this.behaviour.Start(this);
        }

        public Position2 Control()
        {
            this.behaviour.Update();
            this.Navigator.Update();
            
            return this.Navigator.GoalPoint;
        }

    }
}

