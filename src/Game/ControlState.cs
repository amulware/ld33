using Bearded.Utilities.Math;

namespace Centipede.Game
{
    struct ControlState
    {
        private readonly float leftRight;
        private readonly float acceleration;

        public float LeftRight { get { return this.leftRight; } }
        public float Acceleration { get { return this.acceleration; } }

        public ControlState(float acceleration, float leftRight)
        {
            this.acceleration = acceleration.Clamped(0, 1);
            this.leftRight = leftRight.Clamped(-1, 1);
        }
    }
}