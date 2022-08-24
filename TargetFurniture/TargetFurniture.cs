using Dalamud.Game.Command;
using Dalamud.Plugin;
using TargetFurniture;

namespace MoveFurniture;
public class TargetFurniture : IDalamudPlugin
{
    public string Name => "Target Furniture";

    private const string cmdConfig = "/tfconfig";
    private const string cmdConfigShort = "/tfcfg";

    public ContextMenuHousing PluginContextMenu;

    private static PluginUI PluginUI = null!;

    public TargetFurniture(DalamudPluginInterface pluginInterface)
    {

        Service.Initialize(pluginInterface);
        Service.Configuration = Configuration.Load();
        Service.PluginInterface!.UiBuilder.Draw += Service.WindowSystem.Draw;
        Service.PluginInterface!.UiBuilder.OpenConfigUi += OnOpenConfigUi;

        Service.Memory = new();

        PluginUI = new PluginUI();

        PluginContextMenu = new();
        PluginContextMenu.Toggle();

        Service.Commands.AddHandler(cmdConfig, new CommandInfo(OnCommand)
        {
            HelpMessage = "Open Config Window"
        });

        Service.Commands.AddHandler(cmdConfigShort, new CommandInfo(OnCommand)
        {
            HelpMessage = "Short Command for the Config Window "
        });
    }

    private void OnCommand(string command, string args)
    {
        OnOpenConfigUi();
    }

    private void OnOpenConfigUi()
        => PluginUI.Toggle();

    public void Dispose()
    {
        Service.Configuration.Save();

        PluginContextMenu.Dispose();
        PluginUI.Dispose();

        Service.Commands.RemoveHandler(cmdConfig);
        Service.Commands.RemoveHandler(cmdConfigShort);
        Service.PluginInterface!.UiBuilder.Draw -= Service.WindowSystem.Draw;
        Service.PluginInterface.UiBuilder.OpenConfigUi -= OnOpenConfigUi;
    }
}
