using System;
[Serializable]
public class InventoryItem
{
    public ItemData data;
    public int stackSize;

    public InventoryItem(ItemData data, int stackSize)
    {
        this.data = data;
        this.stackSize = stackSize;
    }

    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
