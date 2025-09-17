using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ItemData data;
    public int stackSize;

    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        AddStack();
    }


    public InventoryItem(ItemData _newItemData, int _StackSize)
    {
        data = _newItemData;
        stackSize = _StackSize;
    }

    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
