using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using amulware.Graphics;
using Bearded.Utilities;
using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;
using OpenTK;

namespace Centipede.Game
{
    public class NavQuad
    {
        public Position2 SW { get; }
        public Position2 SE { get; }
        public Position2 NW { get; }
        public Position2 NE { get; }

        public Position2 Center { get; }

        private readonly List<NavLink> links = new List<NavLink>();
        public ReadOnlyCollection<NavLink> Links { get; }

        private readonly Difference2 uDiff1;
        private readonly Difference2 vDiff1;
        private readonly Difference2 uDiff2;
        private readonly Difference2 vDiff2;

        private Position2 uvBase1 => this.SW;
        private Position2 uvBase2 => this.NE;

        private readonly Squared<Unit> area1;
        private readonly Squared<Unit> area2;

        public Squared<Unit> Area  => this.area1 + this.area2;

        public NavQuad(Position2 SW, Position2 SE, Position2 NW, Position2 NE)
        {
            this.SW = SW;
            this.SE = SE;
            this.NW = NW;
            this.NE = NE;

            this.Center = new Position2((SW.NumericValue + SE.NumericValue + NW.NumericValue + NE.NumericValue) / 4);

            this.uDiff1 = SE - SW;
            this.vDiff1 = NW - SW;
            this.uDiff2 = NW - NE;
            this.vDiff2 = SE - NE;

            this.area1 = triangleArea(this.uDiff1, this.vDiff1);
            this.area2 = triangleArea(this.uDiff2, this.vDiff2);

            this.Links = this.links.AsReadOnly();
        }

        private static Squared<Unit> triangleArea(Difference2 u, Difference2 v)
        {
            var a = u.NumericValue;
            var b = v.NumericValue;
            return Squared<Unit>.FromValue(Math.Abs(a.X * b.Y - a.Y * b.X) * 0.5f);
        }

        public void Add(NavLink link)
        {
#if DEBUG
            if (link.From != this)
                throw new Exception("Nav link must originate from this quad.");
#endif
            this.links.Add(link);
        }

        public bool IsInside(Position2 point)
        {
            var uv = this.pointToUV1(point);

            if (uv.X + uv.Y <= 1 && uv.X >= 0 && uv.Y >= 0)
                return true;

            uv = this.pointToUV2(point);

            return uv.X + uv.Y <= 1 && uv.X >= 0 && uv.Y >= 0;
        }

        public Position2 RandomPointInside()
        {
            var uv = new Vector2(StaticRandom.Float(), StaticRandom.Float());

            return this.uvToPoint(uv);
        }
        public Position2 RandomPointInsideUniform()
        {
            var uv = new Vector2(StaticRandom.Float(), StaticRandom.Float());

            var isTriangle2 = uv.X + uv.Y > 1;
            var needsToBeTriangle2 = StaticRandom.Float() < this.area2 / this.Area;

            if (isTriangle2 != needsToBeTriangle2)
            {
                uv = new Vector2(1) - uv;
            }

            return this.uvToPoint(uv);
        }

        private Vector2 pointToUV1(Position2 point)
        {
            var diff = (point - this.uvBase1).NumericValue;

            var u = Vector2.Dot(diff, this.uDiff1.NumericValue);
            var v = Vector2.Dot(diff, this.vDiff1.NumericValue);

            return new Vector2(u, v);
        }

        private Vector2 pointToUV2(Position2 point)
        {
            var diff = (point - this.uvBase2).NumericValue;

            var u = Vector2.Dot(diff, this.uDiff2.NumericValue);
            var v = Vector2.Dot(diff, this.vDiff2.NumericValue);

            return new Vector2(u, v);
        }

        private Position2 uvToPoint(Vector2 uv)
        {
            if (uv.X + uv.Y <= 1)
            {
                return this.uvBase1 + uv.X * this.uDiff1 + uv.Y * this.vDiff1;
            }
            return this.uvBase2 + (1 - uv.X) * this.uDiff2 + (1 - uv.Y) * this.vDiff2;
        }

        public void Draw(IndexedSurface<PrimitiveVertexData> surface)
        {
            var argb = Color.OrangeRed;
            surface.AddQuad(
                new PrimitiveVertexData(this.SW.NumericValue.WithZ(), argb),
                new PrimitiveVertexData(this.SE.NumericValue.WithZ(), argb),
                new PrimitiveVertexData(this.NE.NumericValue.WithZ(), argb),
                new PrimitiveVertexData(this.NW.NumericValue.WithZ(), argb)
            );

            argb = Color.Yellow;
            foreach (var link in this.links)
            {
                var p0 = link.P0.NumericValue.WithZ();
                var p1 = link.P1.NumericValue.WithZ();
                var p2 = link.To.Center.NumericValue.WithZ();

                var d = (p0 - p1).Normalized() * 0.1f;
                var c = (p0 + p1) / 2;

                p0 = c - d;
                p1 = c + d;

                surface.AddTriangle(
                    new PrimitiveVertexData(p0, argb),
                    new PrimitiveVertexData(p1, argb),
                    new PrimitiveVertexData(p2, argb)
                );
            }
        }
    }
}

