using System.Diagnostics.CodeAnalysis;
using Dalamud.Data;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Keys;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.Game.Gui.Toast;
using Dalamud.Game.Libc;
using Dalamud.IoC;
using Dalamud.Plugin;
using XivCommon;

namespace MoveFurniture {
   
    public class Service {
        #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public static void Initialize(DalamudPluginInterface pluginInterface)
            => pluginInterface.Create<Service>();
       
        [PluginService] public static DalamudPluginInterface PluginInterface { get; private set; }
        [PluginService] public static ChatGui Chat { get; private set; }
        [PluginService] public static CommandManager Commands { get; private set; }
        [PluginService] public static GameGui GameGui { get; private set; }
        [PluginService] public static SigScanner SigScanner { get; private set; }

        public static XivCommonBase Common;

        public static PluginMemory Memory;

        public static Configuration configuration;
    }
}
