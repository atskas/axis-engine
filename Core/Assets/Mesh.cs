using UntitledEngine.Core;

namespace UntitledEngine.Core.Assets;

public class Mesh
{
    public float[] Vertices {  get; set; }
    public uint[] Indices {  get; set; }
    public int VertexStride => 4 * sizeof(float); // 4 floats per vertex (2 for position, 2 for UV)
}
