using Dalamud.Game;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using System.Reflection;
using XivCommon;

namespace MoveFurniture {
    public class TargetFurniture : IDalamudPlugin {
        public string Name => "Target Furniture";

        private const string commandName = "/tfconfig";

        private DalamudPluginInterface PluginInterface { get; init; }
      
        private PluginUI PluginUi { get; init; }

        public ContextMenu PluginContextMenu;

        public TargetFurniture(DalamudPluginInterface pluginInterface) {

            Service.Initialize(pluginInterface);
            PluginInterface = pluginInterface;

            Service.configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            Service.configuration.Initialize(PluginInterface);

            PluginUi = new PluginUI();

           /* Service.Commands.AddHandler(commandName, new CommandInfo(OnCommand) {
                HelpMessage = "A useful message to display in /xlhelp"
            });*/

            PluginInterface.UiBuilder.Draw += DrawUI;
            PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;

            Service.Common = new XivCommonBase(Hooks.ContextMenu);
            Service.Memory = new();

            PluginContextMenu = new ContextMenu(this);
            PluginContextMenu.Toggle();
        }

        public void Dispose() {
            PluginUi.Dispose();
            Service.Commands.RemoveHandler(commandName);
            PluginContextMenu.Dispose();
            Service.Common.Dispose();
        }

        private void OnCommand(string command, string args) {
            // in response to the slash command, just display our main ui
            PluginUi.Visible = true;
        }

        private void DrawUI() {
         
            PluginUi.Draw();
        }

        private void DrawConfigUI() {
            PluginUi.SettingsVisible = false;
        }
    }
}
