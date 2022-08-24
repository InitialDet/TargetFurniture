using Dalamud.Logging;
using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace MoveFurniture;

public class PluginMemory
{
    // Layout and housing module pointers.
    private readonly IntPtr layoutWorldPtr;
    public unsafe LayoutWorld* Layout => (LayoutWorld*)layoutWorldPtr;
    public unsafe HousingStructure* HousingStructure => Layout->HousingStruct;

    // Function for selecting an item, usually used when clicking on one in game.
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SelectItemDelegate(IntPtr housingStruct, IntPtr item);
    public SelectItemDelegate SelectItem = null!;

    public PluginMemory()
    {
        try
        {
            // Pointers for housing structures.
            layoutWorldPtr = Service.SigScanner.GetStaticAddressFromSig("48 8B 0D ?? ?? ?? ?? 48 85 C9 74 ?? 48 8B 49 40 E9 ?? ?? ?? ??", 2);

            // Read the pointers.
            layoutWorldPtr = Marshal.ReadIntPtr(layoutWorldPtr);

            // Select housing item.
            IntPtr selectItemAddress = Service.SigScanner.ScanText("E8 ?? ?? ?? ?? 48 8B CE E8 ?? ?? ?? ?? 48 8B 6C 24 40 48 8B CE");
            SelectItem = Marshal.GetDelegateForFunctionPointer<SelectItemDelegate>(selectItemAddress);

        }
        catch (Exception ex)
        {
            PluginLog.LogError(ex, "Error while calling PluginMemory.ctor()");
        }
    }
}

public enum HousingLayoutMode
{
    None,
    Move,
    Rotate,
    Store,
    Place,
    Remove = 6
}

public enum ItemState
{
    None = 0,
    Hover,
    SoftSelect,
    Active
}

public enum ItemState2
{
    None = 0,
    SoftSelect = 3,
    Active = 5
}

[StructLayout(LayoutKind.Explicit)]
public unsafe struct LayoutWorld
{
    [FieldOffset(0x40)] public HousingStructure* HousingStruct;
}

[StructLayout(LayoutKind.Explicit)]
public unsafe struct HousingStructure
{
    [FieldOffset(0x0)] public HousingLayoutMode Mode;
    [FieldOffset(0x4)] public HousingLayoutMode LastMode;
    [FieldOffset(0x8)] public ItemState State;
    [FieldOffset(0xC)] public ItemState2 State2;
    [FieldOffset(0x10)] public HousingItem* HoverItem;
    [FieldOffset(0x18)] public HousingItem* ActiveItem;
    [FieldOffset(0xB8)] public bool Rotating;
}

[StructLayout(LayoutKind.Explicit)]
public unsafe struct HousingItem
{
    [FieldOffset(0x50)] public Vector3 Position;
    [FieldOffset(0x60)] public Quaternion Rotation;
    // [FieldOffset(0x90)] public HousingItemUnknown1* unknown;
}
