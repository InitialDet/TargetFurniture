using System;
using Dalamud.Logging;
using System.Threading.Tasks;
using Dalamud.ContextMenu;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;

namespace MoveFurniture;
public unsafe class ContextMenuHousing : IDisposable
{
    DalamudContextMenu ContextMenu = null!;

    GameObjectContextMenuItem? contextMenuItem;
    AwaitingTarget waiting = new();

    public void Toggle()
    {
        ContextMenu = new DalamudContextMenu();
        contextMenuItem = new GameObjectContextMenuItem(new SeString(new TextPayload("Target Item")), SetFurnitureActive, false);
        ContextMenu.OnOpenGameObjectContextMenu += OnOpenContextMenu;
    }

    public void Dispose()
    {
        if (ContextMenu != null)
            ContextMenu.OnOpenGameObjectContextMenu -= OnOpenContextMenu;
    }

    private void OnOpenContextMenu(GameObjectContextMenuOpenArgs args)
    {

        if (args.ParentAddonName is "HousingGoods" && contextMenuItem != null) // To make sure it doenst appear in the Stored tab
            args.AddCustomItem(contextMenuItem);
    }

    private void SetFurnitureActive(GameObjectContextMenuItemSelectedArgs args)
    {
        if (Service.Memory.HousingStructure->Mode == HousingLayoutMode.Rotate && Service.Memory.HousingStructure->State == ItemState.SoftSelect)
        {
            Service.Memory.SelectItem((IntPtr)Service.Memory.HousingStructure, (IntPtr)Service.Memory.HousingStructure->ActiveItem);
        }
        else if (Service.Memory.HousingStructure->Mode == HousingLayoutMode.Move && Service.Memory.HousingStructure->State == ItemState.SoftSelect)
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
                    waiting.waitingRetarget(this);
                PluginLog.Debug($"Finished");
            }
        }
    }

    public void TargetItem()
    {
        Service.Memory.SelectItem((IntPtr)Service.Memory.HousingStructure, (IntPtr)Service.Memory.HousingStructure->ActiveItem);
    }
}

public class AwaitingTarget
{
    public async void waitingRetarget(ContextMenuHousing contextMenuHousing)
    {
        await Task.Delay(60);
        contextMenuHousing.TargetItem();
    }
}

