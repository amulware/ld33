
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bearded.Utilities.Input;
using Bearded.Utilities.SpaceTime;
using Centipede.Rendering;
using OpenTK.Input;

namespace Centipede.Game
{
    class NavMesh : GameObject
    {
        private bool debugDraw;

        private readonly List<NavQuad> navQuads = new List<NavQuad>();

        public ReadOnlyCollection<NavQuad> NavQuads { get; }
        
        public NavMesh(GameState game)
            : base(game)
        {
            this.NavQuads = this.navQuads.AsReadOnly();
            
            this.isSingleton<NavMesh>();
        }

        public void Add(NavQuad quad)
        {
            this.navQuads.Add(quad);
        }

        public override void Draw()
        {
            if (!this.debugDraw)
                return;

            var surface = SurfaceManager.Instance.NavMesh;
            
            foreach (var quad in this.navQuads)
            {
                quad.Draw(surface);
            }
        }

        public override void Update(TimeSpan elapsedTime)
        {
            if (InputManager.IsKeyHit(Key.N))
                this.debugDraw = !this.debugDraw;
        }
    }
}

