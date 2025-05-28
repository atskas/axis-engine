using ImGuiNET;
using System.Numerics;
using UntitledEngine.Core.ECS;

namespace UntitledEngine.Core.UI;

public class EngineEditor
{
    public static Entity SelectedEntity;
    
    private readonly EntityPanel _entityPanel = new();
    private readonly InspectorPanel _inspectorPanel = new();

    public EngineEditor()
    {
        // Ensure ImGui is initialized only once
        if (ImGui.GetCurrentContext() == IntPtr.Zero)
        {
            ImGui.CreateContext();
            ImGui.SetCurrentContext(ImGui.GetCurrentContext());
        }
        
        var io = ImGui.GetIO();
        io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;
    }
    
    public void UpdateUI()
    {
        ImGui.Begin("Entity List");
        
        // Draw all panels and apply theme
        ApplyBeautifulTheme();
        DrawPanels();

        ImGui.End();
    }
    
    private void DrawPanels()
    {
        _entityPanel.Draw();
        _inspectorPanel.Draw();
    }
    
    private void ApplyBeautifulTheme()
    {
        var style = ImGui.GetStyle();
        var colors = style.Colors;

        // Set rounding and spacing
        style.FrameRounding = 5.0f;
        style.GrabRounding = 5.0f;
        style.ScrollbarRounding = 5.0f;
        style.WindowRounding = 5.0f;
        style.FramePadding = new Vector2(4, 3);
        style.ItemSpacing = new Vector2(6, 4);
        style.IndentSpacing = 20.0f;

        // Base Colors (dark)
        colors[(int)ImGuiCol.WindowBg] = new Vector4(0.13f, 0.14f, 0.15f, 1.00f);
        colors[(int)ImGuiCol.Header] = new Vector4(0.18f, 0.20f, 0.23f, 1.00f);
        colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.28f, 0.29f, 0.31f, 1.00f);
        colors[(int)ImGuiCol.HeaderActive] = new Vector4(0.25f, 0.26f, 0.28f, 1.00f);

        colors[(int)ImGuiCol.Button] = new Vector4(0.20f, 0.22f, 0.25f, 1.00f);
        colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.30f, 0.32f, 0.35f, 1.00f);
        colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.25f, 0.27f, 0.30f, 1.00f);

        colors[(int)ImGuiCol.FrameBg] = new Vector4(0.17f, 0.18f, 0.20f, 1.00f);
        colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.21f, 0.22f, 0.25f, 1.00f);
        colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.24f, 0.25f, 0.29f, 1.00f);

        colors[(int)ImGuiCol.TitleBg] = new Vector4(0.13f, 0.14f, 0.15f, 1.00f);
        colors[(int)ImGuiCol.TitleBgActive] = new Vector4(0.16f, 0.17f, 0.19f, 1.00f);
        colors[(int)ImGuiCol.TitleBgCollapsed] = new Vector4(0.13f, 0.14f, 0.15f, 1.00f);

        colors[(int)ImGuiCol.Text] = new Vector4(0.86f, 0.87f, 0.88f, 1.00f);
        colors[(int)ImGuiCol.TextDisabled] = new Vector4(0.45f, 0.45f, 0.48f, 1.00f);
    }
}

