using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;
using UntitledEngine.Common;

namespace UntitledEngine.Common.Assets;

public class Mesh
{
    public float[] Vertices {  get; set; }
    public uint[] Indices {  get; set; }
    public int VertexStride => 4 * sizeof(float); // 4 floats per vertex (2 for position, 2 for UV)
}
