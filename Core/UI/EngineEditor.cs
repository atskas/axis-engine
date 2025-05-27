using ImGuiNET;
using System.Numerics;

namespace UntitledEngine.Core.UI
{
    public class EngineEditor
    {
        public EngineEditor()
        {
            ImGui.CreateContext();
            ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;
            ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.ViewportsEnable;
        }

        public void UpdateUI()
        {
            ImGui.Begin("Engine Editor");

            ImGui.Text("Welcome to the editor!");
            // Add any editor UI widgets here

            ImGui.End();
        }
    }
}