using Dalamud.Game;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace TargetFurniture;

public class Service
{
    public const string PluginName = "Target Furniture";

    [PluginService] public static DalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] public static ISigScanner SigScanner { get; private set; } = null!;
    [PluginService] public static ICommandManager Commands { get; private set; } = null!;

    public static PluginMemory Memory { get; set; } = null!;
    public static Configuration Configuration { get; set; } = null!;
    public static WindowSystem WindowSystem { get; } = new(PluginName);

    public static void Initialize(DalamudPluginInterface pluginInterface)
    {
        pluginInterface.Create<Service>();
    }
}