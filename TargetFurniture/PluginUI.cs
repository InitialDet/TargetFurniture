using Dalamud.Interface.Windowing;
using ImGuiNET;
using System;
using System.Diagnostics;

namespace TargetFurniture;
public class PluginUi : Window, IDisposable
{
    public PluginUi() : base($"{Service.PluginName} Settings")
    {
        Service.WindowSystem.AddWindow(this);

        Flags |= ImGuiWindowFlags.NoScrollbar;
        Flags |= ImGuiWindowFlags.NoScrollWithMouse;
        Flags |= ImGuiWindowFlags.AlwaysAutoResize;
    }

    public void Dispose()
    {
        Service.Configuration.Save();
        Service.WindowSystem.RemoveWindow(this);
    }

    public override void Draw()
    {
        if (!IsOpen)
            return;

        ShowKofi();

        Utils.Draw.Checkbox("Move Furniture to Cursor", ref Service.Configuration.MoveToCursor, "- If Enabled, the item will follow the point of your cursor.\n- If Disabled, the item will stay in place and move relative to your cursor position." +
            "\n\nIts recommended to have this enabled\n\nDoesn't affect the behavior of alternative mode.");

        ImGui.Spacing();

        ImGui.Text("(Experimental) ");
        Utils.Draw.Checkbox("Enable Alternative Targeting Mode", ref Service.Configuration.UseAltTarget, "(Only for Layout Mode - Move)\nIf the default targeting mode doesn't work for you, this option might work.\n\nThis is a experimental feature and might also not work for everybody\n\nPS: After moving an item, you may see an Error Message at the top, you can ignore it.");

        ImGui.End();
    }

    private static void ShowKofi()
    {
        const string buttonText = "Support on Ko-fi";
        ImGui.PushStyleColor(ImGuiCol.Button, 0xFF000000 | 0x005E5BFF);
        ImGui.PushStyleColor(ImGuiCol.ButtonActive, 0xDD000000 | 0x005E5BFF);
        ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 0xAA000000 | 0x005E5BFF);

        if (ImGui.Button(buttonText))
        {
            Process.Start(new ProcessStartInfo { FileName = "https://ko-fi.com/initialdet", UseShellExecute = true });
        }

        ImGui.PopStyleColor(3);
    }

    public override void OnClose()
    {
        Service.Configuration.Save();
    }
}
