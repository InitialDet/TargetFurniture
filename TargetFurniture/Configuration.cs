﻿using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace MoveFurniture {
    [Serializable]
    public class Configuration : IPluginConfiguration {
        public int Version { get; set; } = 0;

        public bool MoveType2 { get; set; } = true;

        // the below exist just to make saving less cumbersome

        [NonSerialized]
        private DalamudPluginInterface? pluginInterface;

        public void Initialize(DalamudPluginInterface pluginInterface) {
            this.pluginInterface = pluginInterface;
        }

        public void Save() {
            pluginInterface!.SavePluginConfig(this);
        }
    }
}
