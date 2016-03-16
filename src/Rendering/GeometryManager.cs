using amulware.Graphics;
using Bearded.Utilities;
using OpenTK;

namespace Centipede.Rendering
{
    sealed class GeometryManager : Singleton<GeometryManager>
    {
        private readonly SpriteSet<UVColorVertexData> sprites;
        public PrimitiveGeometry Primitives { get; }
        public PrimitiveGeometry PrimitivesOverlay { get; }
        public FontGeometry Text { get; }
        public BuildingGeometry Buildings { get; }

        public GeometryManager(SurfaceManager surfaces)
        {
            this.Primitives = new PrimitiveGeometry(surfaces.Primitives);
            this.PrimitivesOverlay = new PrimitiveGeometry(surfaces.PrimitivesOverlay);

            var font = Font.FromJsonFile("data/fonts/inconsolata.json");
            this.Text = new FontGeometry(surfaces.Text, font) {SizeCoefficient = new Vector2(1, -1)};

            this.Buildings = new BuildingGeometry(surfaces.Buildings);

            this.sprites = surfaces.Sprites;
        }

        public Sprite GetSprite(string name)
        {
            return this.sprites[name];
        }
    }
}