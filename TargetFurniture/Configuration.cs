using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace MoveFurniture {
    [Serializable]
    public class Configuration : IPluginConfiguration {
        public int Version { get; set; } = 1;

        public void Save() {
            Service.PluginInterface!.SavePluginConfig(this);
        }
    }
}
