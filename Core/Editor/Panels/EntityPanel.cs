using System.Numerics;
using ImGuiNET;

namespace UntitledEngine.Core.UI;

public class EntityPanel
{
    public void Draw()
    {
        ImGui.BeginChild("Entity List", new Vector2(200, 0), ImGuiChildFlags.None);

        foreach (var entity in Engine.Instance.SceneManager.CurrentScene.Entities)
        {
            string label = $"{entity.Name}##{entity.GetHashCode()}";
            if (ImGui.Selectable(label, EngineEditor.SelectedEntity == entity))
            {
                EngineEditor.SelectedEntity = entity;
            }
        }

        ImGui.EndChild();
    }
}