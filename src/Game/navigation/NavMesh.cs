
using System.Collections.Generic;
using Bearded.Utilities.SpaceTime;
using Centipede.Rendering;

namespace Centipede.Game
{
    class NavMesh : GameObject
    {
        private readonly List<NavQuad> navQuads = new List<NavQuad>();
        
        public NavMesh(GameState game)
            : base(game)
        {
        }

        public void Add(NavQuad quad)
        {
            this.navQuads.Add(quad);
        }

        public override void Draw()
        {
            var surface = SurfaceManager.Instance.NavMesh;
            
            foreach (var quad in this.navQuads)
            {
                quad.Draw(surface);
            }
        }

        public override void Update(TimeSpan elapsedTime)
        {
        }
    }
}

