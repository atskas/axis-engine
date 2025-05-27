using ImGuiNET;
using UntitledEngine.Core.Components;

namespace UntitledEngine.Core.UI;

public class InspectorPanel
{
    public void Draw()
    {
        ImGui.Begin("Properties");

        var selected = EngineEditor.SelectedEntity;
        if (selected == null)
        {
            ImGui.Text("No entity selected.");
            ImGui.End();
            return;
        }

        ImGui.Text($"Name: {selected.Name}");

        var pos = selected.Transform.Position;
        if (ImGui.DragFloat2("Position", ref pos))
        {
            selected.Transform.Position = pos;
        }

        float rot = selected.Transform.Rotation;
        if (ImGui.DragFloat("Rotation", ref rot))
            selected.Transform.Rotation = rot;

        var scale = selected.Transform.Scale;
        if (ImGui.DragFloat2("Scale", ref scale))
        {
            selected.Transform.Scale = scale;
            var pb = selected.GetComponent<PhysicsBody>();
            if (pb != null)
            {
                pb.ShapeScale = selected.Transform.Scale;
            }
        }

        ImGui.End();
    }
}