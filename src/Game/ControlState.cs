using Bearded.Utilities.Math;

namespace Centipede.Game
{
    struct ControlState
    {
        public float LeftRight { get; }
        public float Acceleration { get; }

        public ControlState(float acceleration, float leftRight)
        {
            this.Acceleration = acceleration.Clamped(0, 1);
            this.LeftRight = leftRight.Clamped(-1, 1);
        }
    }
}