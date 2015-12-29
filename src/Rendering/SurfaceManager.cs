using amulware.Graphics;
using amulware.Graphics.ShaderManagement;
using Bearded.Utilities;

namespace Centipede.Rendering
{
    sealed class SurfaceManager : Singleton<SurfaceManager>
    {
        public Matrix4Uniform ProjectionMatrix { get; private set; }
        public Matrix4Uniform ModelviewMatrix { get; private set; }

        public IndexedSurface<PrimitiveVertexData> Primitives { get; private set; }
        public IndexedSurface<UVColorVertexData> Text { get; private set; }

        public IndexedSurface<BuildingVertex> Buildings { get; private set; }

        public SpriteSet<UVColorVertexData> Sprites { get; private set; } 

        public SurfaceManager(ShaderManager shaderMan)
        {
            // matrices
            this.ProjectionMatrix = new Matrix4Uniform("projectionMatrix");
            this.ModelviewMatrix = new Matrix4Uniform("modelviewMatrix");

            // create shaders
            shaderMan.MakeShaderProgram("primitives");
            shaderMan.MakeShaderProgram("uvcolor");
            shaderMan.MakeShaderProgram("building");

            var sharedSettings = new SurfaceSetting[] {this.ProjectionMatrix, this.ModelviewMatrix};

            // surfaces
            this.Primitives = new IndexedSurface<PrimitiveVertexData>();
            this.Primitives.AddSettings(sharedSettings);
            shaderMan["primitives"].UseOnSurface(this.Primitives);

            this.Text = new IndexedSurface<UVColorVertexData>();
            this.Text.AddSettings(this.ProjectionMatrix, this.ModelviewMatrix,
                new TextureUniform("diffuseTexture", new Texture("data/fonts/inconsolata.png", true)));
            shaderMan["uvcolor"].UseOnSurface(this.Text);

            this.Buildings = new IndexedSurface<BuildingVertex>();
            this.Buildings.AddSettings(sharedSettings);
            shaderMan["building"].UseOnSurface(this.Buildings);

            this.Sprites = SpriteSet<UVColorVertexData>.FromJsonFile("data/sprites/particles.json",
                s => new Sprite2DGeometry(s), shaderMan["uvcolor"], sharedSettings, f => new Texture(f, true), true);
        }

    }
}