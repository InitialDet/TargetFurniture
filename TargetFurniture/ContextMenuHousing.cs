using System;
using XivCommon.Functions.ContextMenu;
using Dalamud.Logging;
using System.Threading.Tasks;

namespace MoveFurniture {
    public unsafe class ContextMenuHousing : IDisposable {
        ContextMenu ContextMenu = null!;

        AwaitingTarget waiting = new();

        public void Toggle() {
            ContextMenu = Service.Common.Functions.ContextMenu;
            ContextMenu.OpenContextMenu += OnOpenContextMenu;
        }

        public void Dispose() {
            if (ContextMenu != null)
                ContextMenu.OpenContextMenu -= OnOpenContextMenu;
        }

        private void OnOpenContextMenu(ContextMenuOpenArgs args) {
            if (args.ParentAddonName is "HousingGoods" && args.Items.Count < 3) // To make sure it doenst appear in the Stored tab
                args.Items.Add(new NormalContextMenuItem("Target Item", SetFurnitureActive));
        }

        private void SetFurnitureActive(ContextMenuItemSelectedArgs args) {
            if (Service.Memory.HousingStructure->Mode == HousingLayoutMode.Rotate && Service.Memory.HousingStructure->State == ItemState.SoftSelect) {
                Service.Memory.SelectItem((IntPtr)Service.Memory.HousingStructure, (IntPtr)Service.Memory.HousingStructure->ActiveItem);
            } else if (Service.Memory.HousingStructure->Mode == HousingLayoutMode.Move && Service.Memory.HousingStructure->State == ItemState.SoftSelect) {
                if (Service.Configuration.UseAltTarget) {
                    Service.Memory.HousingStructure->State = ItemState.Active;
                    Service.Memory.HousingStructure->State2 = ItemState2.Active;
                } else {
                    // To make the item follow the cursor you need to retarget it, for reasons idk you need a small delay before the second target for it to work
                    TargetItem();
                    if (Service.Configuration.MoveToCursor)
                        waiting.waitingRetarget(this);
                    PluginLog.Debug($"Finished");
                }
            }
        }

        public void TargetItem() {
            Service.Memory.SelectItem((IntPtr)Service.Memory.HousingStructure, (IntPtr)Service.Memory.HousingStructure->ActiveItem);
        }
    }

    public class AwaitingTarget {
        public async void waitingRetarget(ContextMenuHousing contextMenuHousing) {
            await Task.Delay(40);
            contextMenuHousing.TargetItem(); 
        }
    }
}
