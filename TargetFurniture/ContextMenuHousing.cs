using System;
using System.Threading.Tasks;
using Dalamud.Game.Gui.ContextMenu;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;

namespace TargetFurniture;

public unsafe class ContextMenuHousing : IDisposable
{
    private MenuItem _menuItemAddConductor;

    public void Dispose()
    {
        Service.ContextMenu.OnMenuOpened -= OnOpenContextMenu;
    }

    public void Toggle()
    {
        _menuItemAddConductor = new MenuItem()
        {
            Name = new SeString(new TextPayload("Target Item")),
            PrefixChar = 'T',
            OnClicked = SetFurnitureActive,
        };
        Service.ContextMenu.OnMenuOpened += OnOpenContextMenu;
    }

    private void OnOpenContextMenu(IMenuOpenedArgs args)
    {
        if (args.AddonName is "HousingGoods" &&
            _menuItemAddConductor != null) // To make sure it doesnt appear in the Stored tab
            args.AddMenuItem(_menuItemAddConductor);
    }

    private void SetFurnitureActive(IMenuItemClickedArgs args)
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