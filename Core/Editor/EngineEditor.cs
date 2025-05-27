using ImGuiNET;
using System.Numerics;
using UntitledEngine.Core.Entities;

namespace UntitledEngine.Core.UI;

public class EngineEditor
{
    public static Entity SelectedEntity;
    
    private readonly EntityPanel _entityPanel = new();
    private readonly InspectorPanel _inspectorPanel = new();

    public void UpdateUI()
    {
        ImGui.Begin("Entity List");
        
        // Draw all panels
        DrawPanels();

        ImGui.End();
    }
    
    
    private void DrawPanels()
    {
        _entityPanel.Draw();
        _inspectorPanel.Draw();
    }
}

