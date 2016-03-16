using System;

namespace Centipede.Game
{
    struct TileRectangle : IEquatable<TileRectangle>
    {
        public TileRectangle(int x0, int y0, int x1, int y1)
        {
            this.X0 = x0;
            this.Y0 = y0;
            this.X1 = x1;
            this.Y1 = y1;
        }

        public int X0 { get; }
        public int Y0 { get; }
        public int X1 { get; }
        public int Y1 { get; }

        public int W => this.X1 - this.X0 + 1;
        public int H => this.Y1 - this.Y0 + 1;

        public int Tiles => this.W * this.H;

        public static TileRectangle Dummy => new TileRectangle(int.MinValue, int.MinValue, int.MinValue, int.MinValue);

        public bool Equals(TileRectangle other)
        {
            return this.X0 == other.X0 && this.Y0 == other.Y0 && this.X1 == other.X1 && this.Y1 == other.Y1;
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
                var hashCode = this.X0;
                hashCode = (hashCode * 397) ^ this.Y0;
                hashCode = (hashCode * 397) ^ this.X1;
                hashCode = (hashCode * 397) ^ this.Y1;
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