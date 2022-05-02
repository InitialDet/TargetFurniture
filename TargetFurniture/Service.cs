using System.Diagnostics.CodeAnalysis;
using Dalamud.Data;
using Dalamud.Game;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.Game.Gui.ContextMenus;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using XivCommon;
using XivCommon.Functions.ContextMenu;

namespace MoveFurniture {

    public class Service {
        public static void Initialize(DalamudPluginInterface pluginInterface)
            => pluginInterface.Create<Service>();

        public const string PluginName = "Target Furniture";

        [PluginService] public static DalamudPluginInterface PluginInterface { get; set; } = null!;
        [PluginService] public static SigScanner SigScanner { get; set; } = null!;
        [PluginService] public static CommandManager Commands { get; private set; } = null!;


        public static PluginMemory Memory { get; set; } = null!;
        public static Configuration Configuration { get; set; } = null!;
        public static XivCommonBase Common { get; set; } = null!;
        public static WindowSystem WindowSystem { get; } = new WindowSystem(PluginName);

    }
}
