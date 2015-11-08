using amulware.Graphics;
using OpenTK;

namespace Centipede.Rendering
{
    struct BuildingVertex : IVertexData
    {
        private readonly Vector3 position;
        private readonly Vector3 normal;
        private readonly Vector2 uv;
        private readonly float alpha;

        public BuildingVertex(Vector3 position, Vector3 normal, Vector2 uv, float alpha = 1)
        {
            this.position = position;
            this.normal = normal;
            this.uv = uv;
            this.alpha = alpha;
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
                VertexData.MakeAttributeTemplate<Vector2>("v_uv"),
                VertexData.MakeAttributeTemplate<float>("v_alpha")
                );
        }

        public int Size()
        {
            return VertexData.SizeOf<BuildingVertex>();
        }
    }
}