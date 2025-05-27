using Box2D.NetStandard.Dynamics.Bodies;
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
        
        var pb = selected.GetComponent<PhysicsBody>(); // Get selected entity's physics body (if it has one)

        ImGui.Text($"Name: {selected.Name}");

        var pos = selected.Transform.Position;
        if (ImGui.DragFloat2("Position", ref pos))
        {
            if (pb != null)
                pb.BodyPosition = pos;
            else
                selected.Transform.Position = pos;
        }

        float rot = selected.Transform.Rotation;
        if (ImGui.DragFloat("Rotation", ref rot))
        {
            if (pb != null)
                pb.BodyRotation = rot;
            else
                selected.Transform.Rotation = rot;
        }
        
        var scale = selected.Transform.Scale;
        if (ImGui.DragFloat2("Scale", ref scale))
        {
            selected.Transform.Scale = scale;
            if (pb != null)
            {
                pb.ShapeScale = selected.Transform.Scale;
            }
        }
        
        if (pb != null)
        {
            var density = pb.Density;
            if (ImGui.DragFloat("Density", ref density))
            {
                pb.Density = density;
            }
        }
        
        if (pb != null)
        {
            var friction = pb.Friction;
            if (ImGui.DragFloat("Friction", ref friction))
            {
                pb.Friction = friction;
            }
        }

        if (pb != null)
        {
            bool isStatic = pb.BodyType == BodyType.Static;
            if (ImGui.Checkbox("Static Body", ref isStatic))
            {
                pb.BodyType = isStatic ? BodyType.Static : BodyType.Dynamic;
            }
        }
        
        if (pb != null)
        {
            bool rotFixed = pb.FixedRotation;
            if (ImGui.Checkbox("Fixed Rotation", ref rotFixed))
            {
                pb.FixedRotation = rotFixed;
                if (pb.Body != null)
                    pb.Body.SetFixedRotation(rotFixed);
            }
        }

        ImGui.End();
    }
}