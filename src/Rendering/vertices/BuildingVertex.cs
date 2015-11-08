using amulware.Graphics;
using OpenTK;

namespace Centipede.Rendering
{
    struct BuildingVertex : IVertexData
    {
        private readonly Vector3 position;
        private readonly Vector3 normal;
        private readonly Vector2 uv;

        public BuildingVertex(Vector3 position, Vector3 normal, Vector2 uv)
        {
            this.position = position;
            this.normal = normal;
            this.uv = uv;
        }

        private static VertexAttribute[] attributes;

        public VertexAttribute[] VertexAttributes()
        {
            return attributes ?? (attributes = makeVertexAttributes());
        }

        private static VertexAttribute[] makeVertexAttributes()
        {
            return VertexData.MakeAttributeArray(
                VertexData.MakeAttributeTemplate<Vector3>("v_position"),
                VertexData.MakeAttributeTemplate<Vector3>("v_normal"),
                VertexData.MakeAttributeTemplate<Vector2>("v_uv")
                );
        }

        public int Size()
        {
            return VertexData.SizeOf<BuildingVertex>();
        }
    }
}