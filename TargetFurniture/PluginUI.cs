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

            Utils.Draw.Checkbox("Enable Alternative Targeting Mode", ref Service.Configuration.UseAltTarget, "(Only for Layout Mode - Move)\nIf the default targeting mode doesn't work for you, this option might work. \n\nPS: After moving an item, you may see an Error Message at the top, you can ignore it.");

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
