using amulware.Graphics;
using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;
using Centipede.Rendering;
using OpenTK;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Centipede.Game.CentipedeParts
{
    class CentiHead : Centipart
    {
        private readonly GameState game;
        private AngularVelocity turnSpeed;
        private Direction2 rotation;
        private Speed speed;

        private Unit distanceTraveled;

        public Direction2 Direction { get { return this.rotation; } }

        public CentiHead(GameState game)
            : base(game)
        {
            this.game = game;
        }

        public void Update(Instant time, TimeSpan elapsedTime, ControlState controlState)
        {
            var leftRight = this.helpSteer(controlState).Clamped(-1, 1);

            this.updateMovement(elapsedTime, controlState.Acceleration, leftRight);
        }

        private float helpSteer(ControlState controlState)
        {
            var leftRight = (
                controlState.LeftRight * 2f
                ).Clamped(-1, 1);

            var rayAngle = Angle.FromRadians(0.7f);
            var rayLength = 2.5.U();

            var vVector = this.Direction.Vector;

            leftRight += this.modifySteering(rayAngle, rayLength, vVector);
            leftRight -= this.modifySteering(-rayAngle, rayLength, vVector);

            return leftRight;
        }

        private float modifySteering(Angle rayAngle, Unit rayLength, Vector2 vVector)
        {
            var ray = new Ray(this.position, (this.Direction + rayAngle) * rayLength)
                .Shoot(this.game);

            if (ray.HasValue)
            {
                var r = ray.Value;

                if (!r.FromInside)
                {
                    var f = (1 - r.RayFactor).Sqrted() * 2.5f;

                    return Vector2.Dot(r.Normal.Vector, vVector) < -0.7f ? f : -f;
                }
            }
            return 0;
        }

        private void updateMovement(TimeSpan elapsedTime, float acceleration, float leftRight)
        {
            var t = (float)elapsedTime.NumericValue;

            this.speed += new Acceleration(acceleration * 50) * elapsedTime;
            this.speed *= Mathf.Pow(1e-3f, t);

            this.turnSpeed += AngularAcceleration.FromRadians(leftRight * 5) * elapsedTime;
            this.turnSpeed *= Mathf.Pow(1e-7f, t);

            this.rotation += this.turnSpeed * (elapsedTime * this.speed.NumericValue);

            this.position += this.rotation * this.speed * elapsedTime;

            this.distanceTraveled += this.speed * elapsedTime;
        }

        public override void Draw()
        {
            var geo = GeometryManager.Instance.Primitives;

            geo.Color = Color.IndianRed.WithAlpha(0.5f);
            geo.DrawCircle(this.position.NumericValue, 1);

            geo.LineWidth = 0.1f;
            geo.DrawLine(this.position.NumericValue, (this.position + this.rotation * 2.U()).NumericValue);

        }

    }
}