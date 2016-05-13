
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

        private readonly Difference2 uDiff;
        private readonly Difference2 vDiff;

        public NavQuad(Position2 SW, Position2 SE, Position2 NW, Position2 NE)
        {
            this.SW = SW;
            this.SE = SE;
            this.NW = NW;
            this.NE = NE;

            this.Center = new Position2((SW.NumericValue + SE.NumericValue + NW.NumericValue + NE.NumericValue) / 4);

            this.uDiff = SE - SW;
            this.vDiff = NW - SW;

            this.Links = this.links.AsReadOnly();
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
            var uv = this.pointToUV(point);

            return uv.X >= 0 && uv.X <= 1 && uv.Y >= 0 && uv.Y <= 1;
        }

        public Position2 RandomPointInside()
        {
            var uv = new Vector2(StaticRandom.Float(), StaticRandom.Float());

            return this.uvToPoint(uv);
        }

        private Vector2 pointToUV(Position2 point)
        {
            var diff = (point - this.SW).NumericValue;

            var u = Vector2.Dot(diff, (this.SE - this.SW).NumericValue);
            var v = Vector2.Dot(diff, (this.NW - this.SW).NumericValue);

            return new Vector2(u, v);
        }

        private Position2 uvToPoint(Vector2 uv)
        {
            return this.SW + uv.X * this.uDiff + uv.Y * this.vDiff;
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

                var d = (p0 - p1).Normalized() * 0.5f;
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

