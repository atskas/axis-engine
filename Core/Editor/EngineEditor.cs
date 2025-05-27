using ImGuiNET;
using System.Numerics;
using UntitledEngine.Core.Entities; // Assumes your Entity class is in this namespace

namespace UntitledEngine.Core.UI
{
// EngineEditor.cs
public class EngineEditor
{
    public static Entity SelectedEntity;
    
    private readonly EntityPanel _entityPanel = new();
    private readonly InspectorPanel _inspectorPanel = new();

    public void UpdateUI()
    {
        ImGui.Begin("Engine Editor");

        _entityPanel.Draw();
        _inspectorPanel.Draw();

        ImGui.End();
    }
}

}