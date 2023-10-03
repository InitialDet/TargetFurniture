using System;
using System.Threading.Tasks;
using Dalamud.ContextMenu;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;

namespace TargetFurniture;

public unsafe class ContextMenuHousing : IDisposable
{
    private DalamudContextMenu _contextMenu = null!;

    private GameObjectContextMenuItem? _contextMenuItem;

    public void Dispose()
    {
        _contextMenu.OnOpenGameObjectContextMenu -= OnOpenContextMenu;
        _contextMenu.Dispose();
    }

    public void Toggle()
    {
        _contextMenu = new DalamudContextMenu(Service.PluginInterface);
        _contextMenuItem =
            new GameObjectContextMenuItem(new SeString(new TextPayload("Target Item")), SetFurnitureActive);
        _contextMenu.OnOpenGameObjectContextMenu += OnOpenContextMenu;
    }

    private void OnOpenContextMenu(GameObjectContextMenuOpenArgs args)
    {
        if (args.ParentAddonName is "HousingGoods" &&
            _contextMenuItem != null) // To make sure it doesnt appear in the Stored tab
            args.AddCustomItem(_contextMenuItem);
    }

    private void SetFurnitureActive(GameObjectContextMenuItemSelectedArgs args)
    {
        if (Service.Memory.HousingStructure->Mode == HousingLayoutMode.Rotate &&
            Service.Memory.HousingStructure->State == ItemState.SoftSelect)
        {
            Service.Memory.SelectItem((IntPtr)Service.Memory.HousingStructure,
                (IntPtr)Service.Memory.HousingStructure->ActiveItem);
        }
        else if (Service.Memory.HousingStructure->Mode == HousingLayoutMode.Move &&
                 Service.Memory.HousingStructure->State == ItemState.SoftSelect)
        {
            if (Service.Configuration.UseAltTarget)
            {
                Service.Memory.HousingStructure->State = ItemState.Active;
                Service.Memory.HousingStructure->State2 = ItemState2.Active;
            }
            else
            {
                // To make the item follow the cursor you need to retarget it, for reasons idk you need a small delay before the second target for it to work
                TargetItem();
                if (Service.Configuration.MoveToCursor)
                    AwaitingTarget.WaitingRetarget();
                //PluginLog.Debug($"Finished");
            }
        }
    }

    public static void TargetItem()
    {
        Service.Memory.SelectItem((IntPtr)Service.Memory.HousingStructure,
            (IntPtr)Service.Memory.HousingStructure->ActiveItem);
    }
}

public static class AwaitingTarget
{
    public static async void WaitingRetarget()
    {
        await Task.Delay(60);
        ContextMenuHousing.TargetItem();
    }
}