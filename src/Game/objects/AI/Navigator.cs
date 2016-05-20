using System;
using Bearded.Utilities.Linq;
using Bearded.Utilities.SpaceTime;

namespace Centipede.Game.AI
{
    class Navigator
    {
        private readonly GameState game;
        private readonly Civilian civilian;

        public NavQuad CurrentQuad { get; private set; }
        private NavLink goalLink;

        public Position2 GoalPoint { get; private set; }

        public Event<NavLink> ReachedNavLink { get; } = new Event<NavLink>();

        public Navigator(GameState game, Civilian civilian)
        {
            this.game = game;
            this.civilian = civilian;

            this.initializePosition();
        }

        private void initializePosition()
        {
            this.CurrentQuad = this.game.Get<NavMesh>().NavQuads.RandomElement();
            this.civilian.SetPosition(CurrentQuad.RandomPointInside());
            this.goalLink = this.CurrentQuad.Links.RandomElement();
            this.updateGoalPoint();
        }

        public void Update()
        {
            if ((this.civilian.Position - this.GoalPoint).LengthSquared < 0.1.U().Squared)
            {
                this.CurrentQuad = this.goalLink.To;
                this.ReachedNavLink.Invoke(this.goalLink);
            }
        }

        public void SetGoalLink(NavLink link)
        {
            if (link.From != this.CurrentQuad)
            {
                throw new Exception("Tried to set navigate to wrong link");
            }
            
            this.goalLink = link;
            this.updateGoalPoint();
        }

        private void updateGoalPoint()
        {
            this.GoalPoint = this.goalLink.RandomPoint();
        }
    }
}

