using System;
using Dalamud.Configuration;

namespace TargetFurniture;
[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 1;

    public void Save()
    {
        Service.PluginInterface.SavePluginConfig(this);
    }

    public bool UseAltTarget = false;

    public bool MoveToCursor = true;

    public static Configuration Load()
    {
        if (Service.PluginInterface.GetPluginConfig() is Configuration config)
        {
            return config;
        }

        config = new Configuration();
        config.Save();
        return config;
    }
}
