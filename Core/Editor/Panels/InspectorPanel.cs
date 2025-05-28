using System.Numerics;
using Box2D.NetStandard.Dynamics.Bodies;
using ImGuiNET;
using UntitledEngine.Core.Components;
using UntitledEngine.Core.ECS;

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
        DrawPlayerControllerSection(selected.GetComponent<PlayerController>(), selected.GetComponent<PhysicsBody>());

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
            var pos = pb.BodyPosition;
            DrawWithReset("Position", ref pos, Vector2.Zero);
            pb.BodyPosition = pos;

            float rot = pb.BodyRotation;
            DrawWithReset("Rotation", ref rot, 0f);
            pb.BodyRotation = rot;
        }
        else
        {
            if (entity.Transform == null)
                return;

            var pos = entity.Transform.Position;
            DrawWithReset("Position", ref pos, Vector2.Zero);
            entity.Transform.Position = pos;

            var rot = entity.Transform.Rotation;
            DrawWithReset("Rotation", ref rot, 0f);
            entity.Transform.Rotation = rot;
        }

        if (entity.Transform != null)
        {
            var scale = entity.Transform.Scale;
            DrawWithReset("Scale", ref scale, Vector2.Zero);
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
        if (ImGui.DragFloat2("Shape Scale", ref shapeScale))
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

    private void DrawPlayerControllerSection(PlayerController controller, PhysicsBody pb)
    {
        if (controller == null)
            return;
        
        if (pb == null)
        {
            ImGui.Separator();
            ImGui.Text("PlayerController requires a PhysicsBody");
            return;
        }
        
        ImGui.Separator();
        ImGui.Text("Player Controller");

        var moveSpeed = controller.moveSpeed;
        if (ImGui.DragFloat("MoveSpeed", ref moveSpeed))
            controller.moveSpeed = moveSpeed;
        
        var jumpPower = controller.jumpPower;
        if (ImGui.DragFloat("JumpPower", ref jumpPower))
            controller.jumpPower = jumpPower;
    }
    
    // Draw a Vector2 or a float with a reset button next to it
    private void DrawWithReset<T>(string label, ref T value, T zeroValue)
    {
        ImGui.PushID(label);

        ImGui.Text(label);
        ImGui.SameLine();

        float dragWidth = ImGui.GetContentRegionAvail().X - 30;
        ImGui.SetNextItemWidth(dragWidth);

        bool changed = false;

        if (typeof(T) == typeof(float))
        {
            float v = Convert.ToSingle(value)!;
            changed = ImGui.DragFloat("##drag", ref v);
            if (changed) value = (T)(object)v!;
        }
        else if (typeof(T) == typeof(Vector2))
        {
            Vector2 v = (Vector2)(object)value!;
            changed = ImGui.DragFloat2("##drag", ref v);
            if (changed) value = (T)(object)v!;
        }
        else
        {
            ImGui.Text("Unsupported type");
        }

        ImGui.SameLine();

        if (ImGui.Button("0"))
        {
            value = zeroValue;
        }

        ImGui.PopID();
    }
}
