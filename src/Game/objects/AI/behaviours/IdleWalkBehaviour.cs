using System;
using Bearded.Utilities.Linq;

namespace Centipede.Game.AI
{
    class IdleWalkBehaviour : BaseBehaviour
    {
        protected override void onStart()
        {
            this.subscribe(this.context.Navigator.ReachedNavLink, this.onReachedNavLink);
        }

        private void onReachedNavLink(NavLink link)
        {
            var oldQuad = link.From;
            var newQuad = link.To;
            if (newQuad.Links.Count > 1)
            {
                while (true)
                {
                    var newLink = newQuad.Links.RandomElement();

                    if (newLink.To != oldQuad)
                    {
                        this.context.Navigator.SetGoalLink(newLink);
                        break;
                    }
                }
            }
            else
            {
                throw new Exception("Could not find outgoing link");
            }
        }

    }

    
}

