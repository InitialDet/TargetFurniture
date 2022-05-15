using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using System;
using Dalamud.Interface.Colors;
using System.Numerics;
using MoveFurniture;

namespace TargetFurniture {
    public class PluginUI : Window, IDisposable {

        public PluginUI() : base($"{Service.PluginName} Settings") {
            Service.WindowSystem.AddWindow(this);

            Flags |= ImGuiWindowFlags.NoScrollbar;
            Flags |= ImGuiWindowFlags.NoScrollWithMouse;
            Flags |= ImGuiWindowFlags.AlwaysAutoResize;
        }

        public void Dispose() {
            Service.Configuration.Save();
            Service.WindowSystem.RemoveWindow(this);
        }

        public override void Draw() {
            if (!IsOpen)
                return;

            Utils.Draw.Checkbox("Move Furniture to Cursor", ref Service.Configuration.MoveToCursor, "- If Enabled, the item will follow the point of your cursor.\n- If Disabled, the item will stay in place and move relative to your cursor position." +
                "\n\nIts recommended to have this enabled\n\nDoesn't affect the behavior of alternative mode.");


            ImGui.Text("(Experimental) ");
            Utils.Draw.Checkbox("Enable Alternative Targeting Mode", ref Service.Configuration.UseAltTarget, "(Only for Layout Mode - Move)\nIf the default targeting mode doesn't work for you, this option might work.\n\nThis is a experimental feature and might also not work for everybody\n\nPS: After moving an item, you may see an Error Message at the top, you can ignore it.");

            ImGui.Indent(28.0f * ImGuiHelpers.GlobalScale);

            ImGui.Indent(-25.0f * ImGuiHelpers.GlobalScale);

            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, ImGuiHelpers.ScaledVector2(10, 8));

            ImGui.Spacing();

            ImGui.End();
        }

        public override void OnClose() {
            Service.Configuration.Save();
        }
    }
}
