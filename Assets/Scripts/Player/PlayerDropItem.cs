using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDropItem : ItemDrop
{
    [Header("Player's drop")]
    [SerializeField] public float chanceToLooseItems;

    public override void GenerateDrop()
    {
        base.GenerateDrop();

        List<InventoryItem> currEquipment = Inventory.instance.GetEquipmentList();
        List<InventoryItem> currStash = Inventory.instance.GetStashList();

        List<InventoryItem> itemToUnquip = new List<InventoryItem>();
        List<InventoryItem> materialToLoose = new List<InventoryItem>();

        if (currEquipment.Count == 0) return;
        foreach (InventoryItem item in currEquipment)
        {
            if(Random.Range(0, 100) < chanceToLooseItems)
            {
                DropItem(item.data);
                itemToUnquip.Add(item);
            }
        }
        for(int i = 0; i < itemToUnquip.Count; i++)
        {
            Inventory.instance.UnequipItem(itemToUnquip[i].data as ItemData_Equipment);
        }

        if (currStash.Count == 0) return;
        foreach (InventoryItem item in currStash)
        {
            if (Random.Range(0, 100) < chanceToLooseItems)
            {
                DropItem(item.data);
                materialToLoose.Add(item);
            }
        }
        for (int i = 0; i < materialToLoose.Count; i++)
        {
            Inventory.instance.RemoveItem(materialToLoose[i].data);
        }
    }
}
