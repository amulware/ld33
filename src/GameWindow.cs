using System;
using amulware.Graphics;
using Bearded.Utilities.Input;
using Centipede.Game;
using Centipede.Rendering;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;

namespace Centipede
{
    sealed class GameWindow : amulware.Graphics.Program
    {
        private GameRenderer renderer;
        private GameState game;

        public GameWindow()
            : base(1280, 720, new GraphicsMode(new ColorFormat(32), 16, 0, 4),
            "The Game", GameWindowFlags.Default, DisplayDevice.Default, 3, 2, GraphicsContextFlags.Default)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            this.renderer = new GameRenderer();

            this.game = new GameState();

            InputManager.Initialize(this.Mouse);
        }

        protected override void OnUpdate(UpdateEventArgs e)
        {
            if (this.Focused)
            {
                InputManager.Update();
            }

            this.game.Update(e);
        }

        protected override void OnRender(UpdateEventArgs e)
        {
            this.renderer.PrepareFrame(this.Width, this.Height);

            this.renderer.Draw(this.game);
            this.renderer.FinaliseFrame();

            if (InputManager.IsKeyHit(Key.P))
            {
                this.SaveScreenshot(Settings.Screenshot.Path, Settings.Screenshot.NameBase, false);
            }

            this.SwapBuffers();
        }
    }
    
}