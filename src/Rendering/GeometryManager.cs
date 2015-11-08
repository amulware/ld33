﻿using amulware.Graphics;
using Bearded.Utilities;
using OpenTK;

namespace Centipede.Rendering
{
    sealed class GeometryManager : Singleton<GeometryManager>
    {
        public PrimitiveGeometry Primitives { get; private set; }
        public FontGeometry Text { get; private set; }
        public BuildingGeometry Buildings { get; private set; }

        public GeometryManager(SurfaceManager surfaces)
        {
            this.Primitives = new PrimitiveGeometry(surfaces.Primitives);

            var font = Font.FromJsonFile("data/fonts/inconsolata.json");
            this.Text = new FontGeometry(surfaces.Text, font) {SizeCoefficient = new Vector2(1, -1)};

            this.Buildings = new BuildingGeometry(surfaces.Buildings);
        }

    }
}