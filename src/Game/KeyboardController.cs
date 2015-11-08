using Bearded.Utilities.Input;
using OpenTK.Input;

namespace Centipede.Game
{
    sealed class KeyboardController
    {
        private readonly IAction forwardAction = KeyboardAction.FromKey(Key.W);
        private readonly IAction leftAction = KeyboardAction.FromKey(Key.A);
        private readonly IAction rightAction = KeyboardAction.FromKey(Key.D);

        public ControlState Control()
        {
            return new ControlState(
                this.forwardAction.AnalogAmount,
                this.leftAction.AnalogAmount - this.rightAction.AnalogAmount
                );
        }
    }
}