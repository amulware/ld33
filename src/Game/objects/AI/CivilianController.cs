
using System;
using Bearded.Utilities.Linq;
using Bearded.Utilities.SpaceTime;

namespace Centipede.Game.AI
{
    class CivilianController
    {
        private readonly Civilian civilian;
        private readonly GameState game;

        private NavQuad currentQuad;
        private NavLink goalLink;

        private Position2 goalPoint;

        public CivilianController(GameState game, Civilian civilian)
        {
            this.game = game;
            this.civilian = civilian;

            this.initializePosition();

            this.updateGoalPoint();
        }

        private void initializePosition()
        {
            this.currentQuad = this.game.Get<NavMesh>().NavQuads.RandomElement();
            this.civilian.SetPosition(currentQuad.RandomPointInside());
            this.goalLink = this.currentQuad.Links.RandomElement();
        }

        public Position2 Control()
        {
            if ((this.civilian.Position - this.goalPoint).LengthSquared < 0.1.U().Squared)
            {
                this.moveToNextRandomNavQuad();
            }

            return this.goalPoint;
        }

        private void moveToNextRandomNavQuad()
        {
            if (this.goalLink.To.Links.Count > 1)
            {
                var oldQuad = this.currentQuad;
                this.currentQuad = this.goalLink.To;
                while (true)
                {
                    this.goalLink = this.currentQuad.Links.RandomElement();

                    if (oldQuad != this.goalLink.To)
                        break;
                }
                this.updateGoalPoint();
            }
        }

        private void updateGoalPoint()
        {
            this.goalPoint = this.goalLink.RandomPoint();
        }
    }
}

