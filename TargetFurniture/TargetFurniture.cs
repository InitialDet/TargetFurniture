using Dalamud.Plugin;
using XivCommon;

namespace MoveFurniture {
    public class TargetFurniture : IDalamudPlugin {
        public string Name => "Target Furniture";

        private const string commandName = "/tfconfig";

        public ContextMenuHousing PluginContextMenu;

        public TargetFurniture(DalamudPluginInterface pluginInterface) {
            
            Service.Initialize(pluginInterface);
            Service.Configuration = Service.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            Service.Common = new XivCommonBase(Hooks.ContextMenu);

            Service.Memory = new();

            PluginContextMenu = new();
            PluginContextMenu.Toggle();
        }

        public void Dispose() {
            PluginContextMenu.Dispose();
            Service.Common.Dispose();
        }
    }
}
