using Dalamud.Game.Command;
using Dalamud.Plugin;

namespace TargetFurniture;

public class TargetFurniture : IDalamudPlugin
{
    //public static string Name => "Target Furniture";

    private const string CmdConfig = "/targetfurniture";
    private const string CmdConfigShort = "/tfcfg";

    private static PluginUi _pluginUi = null!;

    private readonly ContextMenuHousing _pluginContextMenu;

    public TargetFurniture(IDalamudPluginInterface pluginInterface)
    {
        Service.Initialize(pluginInterface);
        Service.Configuration = Configuration.Load();
        Service.PluginInterface.UiBuilder.Draw += Service.WindowSystem.Draw;
        Service.PluginInterface.UiBuilder.OpenConfigUi += OnOpenConfigUi;
        Service.PluginInterface.UiBuilder.OpenMainUi += OnOpenConfigUi; 

        Service.Memory = new PluginMemory();

        _pluginUi = new PluginUi();

        _pluginContextMenu = new ContextMenuHousing();
        _pluginContextMenu.Toggle();

        Service.Commands.AddHandler(CmdConfig, new CommandInfo(OnCommand)
        {
            HelpMessage = "Open Config Window"
        });

        Service.Commands.AddHandler(CmdConfigShort, new CommandInfo(OnCommand)
        {
            HelpMessage = "Short Command for the Config Window "
        });
    }

    public void Dispose()
    {
        Service.Configuration.Save();

        _pluginContextMenu.Dispose();
        _pluginUi.Dispose();

        Service.Commands.RemoveHandler(CmdConfig);
        Service.Commands.RemoveHandler(CmdConfigShort);
        Service.PluginInterface.UiBuilder.Draw -= Service.WindowSystem.Draw;
        Service.PluginInterface.UiBuilder.OpenConfigUi -= OnOpenConfigUi;
        Service.PluginInterface.UiBuilder.OpenMainUi -= OnOpenConfigUi;
    }

    private static void OnCommand(string command, string args)
    {
        OnOpenConfigUi();
    }

    private static void OnOpenConfigUi()
    {
        _pluginUi.Toggle();
    }
}