using System;
using System.Diagnostics;
using amulware.Graphics;
using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;
using Centipede.Rendering;
using OpenTK;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Centipede.Game.CentipedeParts
{
    class CentiHead
    {
        private readonly GameState game;
        private Position2 position;
        private AngularVelocity turnSpeed;
        private Direction2 rotation;
        private Speed speed;

        private Unit distanceTraveled;

        public Position2 Position { get { return this.position; } }
        public Direction2 Direction { get { return this.rotation; } }

        public CentiHead(GameState game)
        {
            this.game = game;
        }

        public void Update(Instant time, TimeSpan elapsedTime, ControlState controlState)
        {
            var leftRight = (
                controlState.LeftRight * 2f
                ).Clamped(-1, 1);

            var rayAngle = Angle.FromRadians(0.7f);
            var rayLength = 2.5f.U();

            var rayLeft = new Ray(this.position, (this.Direction + rayAngle) * rayLength)
                .Shoot(this.game);
            var rayRight = new Ray(this.position, (this.Direction - rayAngle) * rayLength)
                .Shoot(this.game);

            var vVector = this.Direction.Vector;

            if (rayLeft.HasValue)
            {
                var r = rayLeft.Value;

                if (!r.FromInside)
                {
                    var f = (1 - r.RayFactor).Sqrted() * 2.5f;

                    if (Vector2.Dot(r.Normal.Vector, vVector) < -0.7f)
                    {
                        leftRight += f;
                    }
                    else
                    {
                        leftRight += -f;
                    }
                }
            }

            if (rayRight.HasValue)
            {
                var r = rayRight.Value;

                if (!r.FromInside)
                {
                    var f = (1 - r.RayFactor).Sqrted() * 2.5f;

                    if (Vector2.Dot(r.Normal.Vector, vVector) < -0.7f)
                    {
                        leftRight += -f;
                    }
                    else
                    {
                        leftRight += f;
                    }
                }
            }

            leftRight = leftRight.Clamped(-1, 1);

            this.updateMovement(elapsedTime, controlState.Acceleration, leftRight);
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

        public void Draw()
        {
            var geo = GeometryManager.Instance.Primitives;

            geo.Color = Color.IndianRed.WithAlpha(0.5f);
            geo.DrawCircle(this.position.NumericValue, 1);

            geo.LineWidth = 0.1f;
            geo.DrawLine(this.position.NumericValue, (this.position + this.rotation * 2.U()).NumericValue);

        }

    }
}