using amulware.Graphics;
using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;

namespace Game
{
    sealed class Building : GameObject
    {
        private Position2 topLeft;
        private Difference2 size;

        public Building(GameState game, Position2 topLeft, Difference2 size)
            : base(game)
        {
            this.topLeft = topLeft;
            this.size = size;

            game.AddBuilding(this);
        }
        
        public override void Update(TimeSpan elapsedTime)
        {
        }

        public HitResult? TryHit(Ray ray)
        {
            var start = ray.Start.Vector;
            var direction = ray.Direction.Vector;

            var topLeft = this.topLeft.Vector;
            var bottomRight = (this.topLeft + this.size).Vector;

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

        public override void Draw()
        {
            var geo = GeometryManager.Instance.Primitives;

            geo.Color = Color.Gray;
            geo.DrawRectangle(this.topLeft.Vector, this.size.Vector);

        }
    }

    struct HitResult
    {
        private readonly Position2 point;
        private readonly Direction2 normal;
        private readonly float rayFactor;
        private readonly bool fromInside;

        public HitResult(Position2 point, Direction2 normal, float rayFactor, bool fromInside)
        {
            this.point = point;
            this.normal = normal;
            this.rayFactor = rayFactor;
            this.fromInside = fromInside;
        }

        public Position2 Point { get { return this.point; } }
        public Direction2 Normal { get { return this.normal; } }
        public float RayFactor { get { return this.rayFactor; } }
        public bool FromInside { get { return this.fromInside; } }
    }
}