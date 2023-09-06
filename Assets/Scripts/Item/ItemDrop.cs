using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int dropAmount;
    [SerializeField] private GameObject dropPrefab;

    [SerializeField] private List<ItemData> dropList;
    [SerializeField] private ItemData[] possibleDrop;

    public virtual void GenerateDrop()
    {
        for(int i = 0; i < possibleDrop.Length; i++)
        {
            if(Random.Range(0, 100) < possibleDrop[i].dropChance)
            {
                Debug.Log(possibleDrop[i].name);
                dropList.Add(possibleDrop[i]);
            }
        }
        if (dropList.Count == 0) return;
        for(int i = 0; i < dropAmount; i++)
        {
            ItemData randomItem = dropList[Random.Range(0, dropList.Count - 1)];
            
            dropList.Remove(randomItem);

            DropItem(randomItem);
        }
    }

    protected void DropItem(ItemData _itemData)
    {
        GameObject dropItem = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        Vector2 randomVelcity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));
        dropItem.GetComponent<ItemObject>().SetupItem(_itemData, randomVelcity);
    }
}
