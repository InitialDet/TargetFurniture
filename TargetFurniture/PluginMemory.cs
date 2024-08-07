﻿using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace TargetFurniture;

public class PluginMemory
{
    // Function for selecting an item, usually used when clicking on one in game.
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SelectItemDelegate(IntPtr housingStruct, IntPtr item);

    // Layout and housing module pointers.
    private readonly IntPtr _layoutWorldPtr;
    public readonly SelectItemDelegate SelectItem = null!;

    public PluginMemory()
    {
        try
        {
            // Pointers for housing structures.
            _layoutWorldPtr =
                Service.SigScanner.GetStaticAddressFromSig(
                    "48 8B D1 48 8B 0D ?? ?? ?? ?? 48 85 C9 74 0A", 3);

            // Read the pointers.
            _layoutWorldPtr = Marshal.ReadIntPtr(_layoutWorldPtr);

            // Select housing item.
            var selectItemAddress =
                Service.SigScanner.ScanText("48 85 D2 0F 84 49 09 00 00 53 41 56 48 83 EC 48 48 89 6C 24 60 48 8B DA 48 89 74 24 70 4C 8B F1");
            SelectItem = Marshal.GetDelegateForFunctionPointer<SelectItemDelegate>(selectItemAddress);
        }
        catch (Exception)
        {
            //PluginLog.LogError(ex, "Error while calling PluginMemory.ctor()");
        }
    }

    private unsafe LayoutWorld* Layout => (LayoutWorld*)_layoutWorldPtr;
    public unsafe HousingStructure* HousingStructure => Layout->HousingStruct;
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
public struct HousingItem
{
    [FieldOffset(0x50)] public Vector3 Position;

    [FieldOffset(0x60)] public Quaternion Rotation;
    // [FieldOffset(0x90)] public HousingItemUnknown1* unknown;
}