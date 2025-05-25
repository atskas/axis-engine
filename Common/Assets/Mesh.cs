using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;
using UntitledEngine.Common;

namespace UntitledEngine.Common.Assets;

public class Mesh
{
    public float[] Vertices {  get; set; }
    public uint[] Indices {  get; set; }
    // Soon add UVs and Texture here
}
