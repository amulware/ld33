using System.Collections.Generic;
using amulware.Graphics;
using Bearded.Utilities.Input;
using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;
using OpenTK.Input;

namespace Game
{
    struct CentiPathPart
    {
        private readonly Position2 position;
        private readonly Radius distanceToPrevious;

        public CentiPathPart(Position2 position, CentiPathPart previous)
        {
            this.position = position;
            this.distanceToPrevious = (position - previous.position).Length;
        }

        public CentiPathPart(Position2 position, Radius distanceToPrevious)
        {
            this.position = position;
            this.distanceToPrevious = distanceToPrevious;
        }

        public Position2 Position { get { return this.position; } }
        public Radius DistanceToPrevious { get { return this.distanceToPrevious; } }
    }

    sealed class Centipede : GameObject
    {
        private CentiHead head;
        private List<Centipart> parts = new List<Centipart>();

        private LinkedList<CentiPathPart> tailPath = new LinkedList<CentiPathPart>();

        private KeyboardController controller = new KeyboardController();

        private CentiPathPart lastSaved;

        public Centipede(GameState game, int length = 15)
            : base(game)
        {
            this.head = new CentiHead();

            this.tailPath.AddFirst(new CentiPathPart(this.head.Position, Radius.FromValue(0)));

            for (int i = 0; i < length; i++)
            {
                this.parts.Add(new Centipart());
            }
        }

        public override void Update(TimeSpan elapsedTime)
        {
            var controlState = this.controller.Control();
            this.head.Update(this.game.Time, elapsedTime, controlState);

            var currentPathPart = new CentiPathPart(this.head.Position, this.lastSaved);

            this.updateParts(currentPathPart);

            if (currentPathPart.DistanceToPrevious > Radius.FromValue(0.5f))
            {
                this.tailPath.AddFirst(currentPathPart);
                this.lastSaved = currentPathPart;
            }

        }

        private void updateParts(CentiPathPart startPosition)
        {
            var intervalStart = startPosition;
            var intervalStartDistance = 0f;

            var intervalLength = intervalStart.DistanceToPrevious.NumericValue;

            var intervalEndNode = this.tailPath.First;


            var i = 0;

            var partDistanceStep = 1.5f;
            var partDistance = partDistanceStep;

            while (true)
            {
                var intervalEnd = intervalEndNode.Value;
                var intervalEndDistance = intervalStartDistance + intervalLength;

                if (intervalLength > 0)
                {
                    while (partDistance < intervalEndDistance)
                    {
                        // position part, move to next
                        var part = this.parts[i];

                        var p = (partDistance - intervalStartDistance) / intervalLength;

                        part.SetPosition(intervalStart.Position + (intervalEnd.Position - intervalStart.Position) * p);

                        partDistance += partDistanceStep;
                        i++;

                        if (i == this.parts.Count)
                        {
                            while (this.tailPath.Last != intervalEndNode)
                                this.tailPath.RemoveLast();
                            
                            return;
                        }
                    }
                }


                if (intervalEndNode.Next == null)
                {
                    for (; i < this.parts.Count; i++)
                    {
                        var part = this.parts[i];
                        part.SetPosition(intervalEnd.Position);
                    }
                    return;
                }

                intervalStart = intervalEnd;
                intervalStartDistance = intervalEndDistance;
                intervalLength = intervalStart.DistanceToPrevious.NumericValue;

                intervalEndNode = intervalEndNode.Next;
            }
        }

        public override void Draw()
        {
            this.head.Draw();
            foreach (var part in this.parts)
            {
                part.Draw();
            }

            // debug draw tail path
#if false

            var p = this.head.Position.Vector;
            foreach (var part in this.tailPath)
            {
                var p2 = part.Position.Vector;

                geo.Color = Color.Green;
                geo.LineWidth = 0.05f;
                geo.DrawLine(p, p2);

                p = p2;
            }
#endif
        }
    }

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

    class CentiHead
    {
        private Position2 position;
        private Angle turnSpeed;
        private Direction2 rotation;
        private Unit speed;

        private Unit distanceTraveled;

        public Position2 Position { get { return this.position; } }
        public Direction2 Direction { get { return this.rotation; } }

        public void Update(Instant time, TimeSpan elapsedTime, ControlState controlState)
        {
            var t = (float)elapsedTime.NumericValue;

            this.speed += new Unit(controlState.Acceleration * t * 50);
            this.speed *= GameMath.Pow(1e-3f, t);

            var leftRight = (
                controlState.LeftRight * 2f +
                GameMath.Sin(this.distanceTraveled.NumericValue * 0.7f) * 0.5f
                ).Clamped(-1, 1);

            this.turnSpeed += Angle.FromRadians(leftRight * t * 5);
            this.turnSpeed *= GameMath.Pow(1e-7f, t);

            this.rotation += this.turnSpeed * (t * this.speed.NumericValue);

            this.position += Difference2.In(this.rotation, speed) * t;

            this.distanceTraveled += speed * t;
        }

        public void Draw()
        {
            var geo = GeometryManager.Instance.Primitives;

            geo.Color = Color.IndianRed;
            geo.DrawCircle(this.position.Vector, 1);

            geo.LineWidth = 0.1f;
            geo.DrawLine(this.position.Vector, (this.position + Difference2.In(this.rotation, Unit.One * 2)).Vector);

        }

    }

    class Centipart
    {
        private Position2 position;

        public Position2 Position
        {
            get { return this.position; }
        }

        public void SetPosition(Position2 position)
        {
            this.position = position;
        }

        public void Draw()
        {
            var geo = GeometryManager.Instance.Primitives;

            geo.Color = Color.IndianRed;
            geo.DrawCircle(this.position.Vector, 0.9f);

        }
    }
}