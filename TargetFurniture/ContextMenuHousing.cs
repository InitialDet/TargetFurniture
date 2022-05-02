using System;
using XivCommon.Functions.ContextMenu;
using Dalamud.Logging;

namespace MoveFurniture {
    public unsafe class ContextMenuHousing : IDisposable {
        ContextMenu ContextMenu = null!;

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
            } 
            else if (Service.Memory.HousingStructure->Mode == HousingLayoutMode.Move && Service.Memory.HousingStructure->State == ItemState.SoftSelect) {
                Service.Memory.HousingStructure->State = ItemState.Active;
                Service.Memory.HousingStructure->State2 = ItemState2.Active;
            }
        }
    }
}
