using System;

namespace Centipede.Game
{
    struct TileRectangle : IEquatable<TileRectangle>
    {
        private readonly int x0;
        private readonly int y0;
        private readonly int x1;
        private readonly int y1;

        public TileRectangle(int x0, int y0, int x1, int y1)
        {
            this.x0 = x0;
            this.y0 = y0;
            this.x1 = x1;
            this.y1 = y1;
        }

        public int X0 { get { return this.x0; } }
        public int Y0 { get { return this.y0; } }
        public int X1 { get { return this.x1; } }
        public int Y1 { get { return this.y1; } }
        public int W { get { return this.x1 - this.x0 + 1; } }
        public int H { get { return this.y1 - this.y0 + 1; } }

        public int Tiles { get { return this.W * this.H; } }

        public static TileRectangle Dummy { get { return new TileRectangle(int.MinValue, int.MinValue, int.MinValue, int.MinValue); } }

        public bool Equals(TileRectangle other)
        {
            return this.x0 == other.x0 && this.y0 == other.y0 && this.x1 == other.x1 && this.y1 == other.y1;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is TileRectangle && Equals((TileRectangle)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.x0;
                hashCode = (hashCode * 397) ^ this.y0;
                hashCode = (hashCode * 397) ^ this.x1;
                hashCode = (hashCode * 397) ^ this.y1;
                return hashCode;
            }
        }

        public static bool operator ==(TileRectangle left, TileRectangle right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TileRectangle left, TileRectangle right)
        {
            return !(left == right);
        }

    }
}