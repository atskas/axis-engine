namespace UntitledEngine.Core.Assets;

public struct Color
{
    private float r, g, b, a;

    public float R
    {
        get => r;
        set => r = Math.Clamp(value, 0f, 1f);
    }
    public float G
    {
        get => g;
        set => g = Math.Clamp(value, 0f, 1f);
    }
    public float B
    {
        get => b;
        set => b = Math.Clamp(value, 0f, 1f);
    }
    public float A
    {
        get => a;
        set => a = Math.Clamp(value, 0f, 1f);
    }

    public Color(float r, float g, float b, float a)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }
}

