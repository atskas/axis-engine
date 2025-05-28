using System;
using System.Numerics;
using ImGuiNET;
using UntitledEngine.Core.Components;

namespace UntitledEngine.Core.UI;

public class EntityPanel
{
    private static string searchBuffer = string.Empty;

    public void Draw()
    {
        ImGui.Begin("Entity List");
        ImGui.BeginChild("Entity Panel", new Vector2(0, 0), ImGuiChildFlags.None, ImGuiWindowFlags.AlwaysVerticalScrollbar);

        // --- Search Bar ---
        ImGui.InputText("Search", ref searchBuffer, 256);
        ImGui.Separator();

        foreach (var entity in Engine.Instance.SceneManager.CurrentScene.Entities)
        {
            if (!string.IsNullOrWhiteSpace(searchBuffer) &&
                !entity.Name.Contains(searchBuffer, StringComparison.OrdinalIgnoreCase))
                continue;

            bool isSelected = EngineEditor.SelectedEntity == entity;
            string label = $"{entity.Name}##{entity.GetHashCode()}";

            if (isSelected)
                ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(1f, 1f, 0.5f, 1f)); // Highlight selected

            if (ImGui.Selectable(label, isSelected))
            {
                EngineEditor.SelectedEntity = entity;
            }

            if (isSelected)
                ImGui.PopStyleColor();

            // Tooltip on hover
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip($"Position: {entity.Transform.Position}");
            }

            // Right-click context menu
            if (ImGui.BeginPopupContextItem(label))
            {
                if (ImGui.MenuItem("Delete"))
                    entity.Destroy();

                ImGui.EndPopup();
            }
        }

        ImGui.EndChild();
        ImGui.End();
    }
}