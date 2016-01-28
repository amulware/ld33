using System;
using Bearded.Utilities;
using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;
using Centipede.Rendering;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Centipede.Game
{
    sealed class Building : GameObject
    {
        private Position2 topLeft;
        private Difference2 size;
        private Unit height = StaticRandom.Float(15, 30).U();

        private float interiorAlpha;

        public bool RevealInside { get; set; }

        public Building(GameState game, Position2 topLeft, Difference2 size)
            : base(game)
        {
            this.topLeft = topLeft;
            this.size = size;

            this.listAs<Building>();
        }
        
        public override void Update(TimeSpan elapsedTime)
        {
            var interiorAlphaGoal = this.RevealInside ? 1 : 0;

            var fade = (float)elapsedTime.NumericValue * 2;

            if (interiorAlphaGoal > this.interiorAlpha)
            {
                this.interiorAlpha = Math.Min(interiorAlphaGoal, this.interiorAlpha + fade);
            }
            else
            {
                this.interiorAlpha = Math.Max(interiorAlphaGoal, this.interiorAlpha - fade);
            }
        }

        public HitResult? TryHit(Ray ray)
        {
            var start = ray.Start.NumericValue;
            var direction = ray.Direction.NumericValue;

            var topLeft = this.topLeft.NumericValue;
            var bottomRight = (this.topLeft + this.size).NumericValue;

            HitResult? result = null;

            var bestF = 1f;

            if (direction.X != 0)
            {
                { // left
                    var f = (topLeft.X - start.X) / direction.X;

                    if (f >= 0 && f < bestF)
                    {
                        var y = start.Y + direction.Y * f;

                        if (y >= topLeft.Y && y <= bottomRight.Y)
                        {
                            bestF = f;
                            result = new HitResult(new Position2(topLeft.X, y), Direction2.FromDegrees(180), f,
                                start.X > topLeft.X);
                        }
                    }
                }
                { // right
                    var f = (bottomRight.X - start.X) / direction.X;

                    if (f >= 0 && f < bestF)
                    {
                        var y = start.Y + direction.Y * f;

                        if (y >= topLeft.Y && y <= bottomRight.Y)
                        {
                            bestF = f;
                            result = new HitResult(new Position2(bottomRight.X, y), Direction2.FromDegrees(0), f,
                                start.X < bottomRight.X);
                        }
                    }
                }
            }

            if (direction.Y != 0)
            {
                { // top
                    var f = (topLeft.Y - start.Y) / direction.Y;

                    if (f >= 0 && f < bestF)
                    {
                        var x = start.X + direction.X * f;

                        if (x >= topLeft.X && x <= bottomRight.X)
                        {
                            bestF = f;
                            result = new HitResult(new Position2(x, topLeft.Y), Direction2.FromDegrees(270), f,
                                start.Y > topLeft.Y);
                        }
                    }
                }
                { // bottom
                    var f = (bottomRight.Y - start.Y) / direction.Y;

                    if (f >= 0 && f < bestF)
                    {
                        var x = start.X + direction.X * f;

                        if (x >= topLeft.X && x <= bottomRight.X)
                        {
                            bestF = f;
                            result = new HitResult(new Position2(x, bottomRight.Y), Direction2.FromDegrees(90), f,
                                start.Y < bottomRight.Y);
                        }
                    }
                }
            }

            return result;
        }

        public bool IsInside(Position2 point)
        {
            return point.X >= this.topLeft.X
                && point.Y >= this.topLeft.Y
                && point.X < this.topLeft.X + this.size.X
                && point.Y < this.topLeft.Y + this.size.Y;
        }

        public override void Draw()
        {
            var geo = GeometryManager.Instance.Buildings;

            geo.DrawBuilding(this.topLeft.NumericValue, this.size.NumericValue, this.height.NumericValue, this.interiorAlpha);

        }
    }
}