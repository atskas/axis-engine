using Box2D.NetStandard.Dynamics.Bodies;
using ImGuiNET;
using UntitledEngine.Core.Components;
using UntitledEngine.Core.Entities;

namespace UntitledEngine.Core.UI;

public class InspectorPanel
{
    public void Draw()
    {
        ImGui.Begin("Inspector");

        var selected = EngineEditor.SelectedEntity;
        if (selected == null)
        {
            ImGui.Text("No entity selected.");
            ImGui.End();
            return;
        }

        DrawEntity(selected);

        DrawTransformSection(selected);
        DrawPhysicsBodySection(selected.GetComponent<PhysicsBody>());

        ImGui.End();
    }

    private void DrawEntity(Entity selected)
    {
        ImGui.Text($"Name: {selected.Name}");
    
        ImGui.SameLine();

        float availWidth = ImGui.GetContentRegionAvail().X;
        float cursorPosX = ImGui.GetCursorPosX();

        float buttonWidth = 25f; // approximate button width in pixels
        float posX = cursorPosX + availWidth - buttonWidth;

        // Move the cursor to the right aligned position
        ImGui.SetCursorPosX(posX);

        if (ImGui.Button("DEL"))
        {
            selected.Destroy();
            EngineEditor.SelectedEntity = null;
        }
    }

    private void DrawTransformSection(Entity entity)
    {
        ImGui.Separator();
        ImGui.Text("Transform");

        var pb = entity.GetComponent<PhysicsBody>();

        if (pb != null)
        {
            // Use physics body position and rotation
            var pos = pb.BodyPosition;
            if (ImGui.DragFloat2("Position", ref pos))
                pb.BodyPosition = pos;

            float rot = pb.BodyRotation;
            if (ImGui.DragFloat("Rotation", ref rot))
                pb.BodyRotation = rot;
        }
        else
        {
            if (entity.Transform == null)
            {
                return;
            }

            var pos = entity.Transform.Position;
            if (ImGui.DragFloat2("Position", ref pos))
                entity.Transform.Position = pos;

            var rot = entity.Transform.Rotation;
            if (ImGui.DragFloat("Rotation", ref rot))
                entity.Transform.Rotation = rot;
        }
        
        if (entity.Transform != null)
        {
            var scale = entity.Transform.Scale;
            if (ImGui.DragFloat2("Scale", ref scale))
                entity.Transform.Scale = scale;
        }
    }


    private void DrawPhysicsBodySection(PhysicsBody pb)
    {
        if (pb == null)
            return;

        ImGui.Separator();
        ImGui.Text("Physics");
        
        var shapeScale = pb.ShapeScale;
        if (ImGui.DragFloat2("Shape (Collider) Scale", ref shapeScale))
            pb.ShapeScale = shapeScale;

        float density = pb.Density;
        if (ImGui.DragFloat("Density", ref density))
            pb.Density = density;

        float friction = pb.Friction;
        if (ImGui.DragFloat("Friction", ref friction))
            pb.Friction = friction;

        bool isStatic = pb.BodyType == BodyType.Static;
        if (ImGui.Checkbox("Static Body", ref isStatic))
            pb.BodyType = isStatic ? BodyType.Static : BodyType.Dynamic;

        bool rotFixed = pb.FixedRotation;
        if (ImGui.Checkbox("Fixed Rotation", ref rotFixed))
            pb.FixedRotation = rotFixed;
    }
}
