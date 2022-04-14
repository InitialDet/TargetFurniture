using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XivCommon;
using System.Threading.Tasks;
using XivCommon.Functions.ContextMenu;
using Dalamud.Logging;
using Dalamud.Hooking;
using FFXIVClientStructs.FFXIV.Component.GUI;
using System.Runtime.InteropServices;
using FFXIVClientStructs.FFXIV.Client.UI;

namespace MoveFurniture {
    public unsafe class ContextMenu : IDisposable {
        
        private TargetFurniture Plugin { get; }

        public ContextMenu(TargetFurniture plugin) {
            Plugin = plugin;
           
        }

        public void Toggle() {
            if (Service.Common != null)
                Service.Common.Functions.ContextMenu.OpenContextMenu += OnOpenContextMenu;
        }

        public void Dispose() {
            Service.Common.Functions.ContextMenu.OpenContextMenu -= OnOpenContextMenu;
        }

        private void OnOpenContextMenu(ContextMenuOpenArgs args) {  
            if (args.ParentAddonName is not "HousingGoods") {
                return;
            }

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
