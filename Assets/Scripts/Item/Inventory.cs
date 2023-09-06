using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Inventory : MonoBehaviour, ISaveManager
{
    public static Inventory instance;

    public List<ItemData> startintEquipment;

    public List<InventoryItem> inventorys;
    public Dictionary<ItemData, InventoryItem> inventoryDict;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDict;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDict;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private UI_ItemSlot[] inventoryItem;
    private UI_ItemSlot[] stashItem;
    private UI_EquipmentSlot[] equipmentItem;
    private UI_StatSlot[] statSlot;

    [Header("Items Cooldown")]
    private float lastTimeUsedFlask;
    public float flaskCooldown { get; private set; }

    private float lastTimeUsedArmor;
    private float armorCooldown;

    [Header("Data Base")]
    public  List<InventoryItem> loadedItems;
    public List<ItemData_Equipment> loadedEquipments;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance.gameObject);
    }

    private void Start()
    {
        inventorys = new List<InventoryItem>();
        inventoryDict = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDict = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDict = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventoryItem = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItem = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentItem = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        statSlot = statSlotParent.GetComponentsInChildren<UI_StatSlot>();

        AddStartingEquipment();
    }

    private void AddStartingEquipment()
    {
        foreach (ItemData_Equipment _equipment in loadedEquipments)
        {
            EquipItem(_equipment);
        }
        if (loadedItems.Count > 0)
        {
            foreach (var item in loadedItems)
            {
                for (int i = 0; i < item.stackSize; i++)
                    AddItem(item.data);
            }
        }

        //for (int i = 0; i < startintEquipment.Count; i++)
        //{
        //    AddItem(startintEquipment[i]);
        //}
    }

    public void EquipItem(ItemData _data)
    {
        ItemData_Equipment newEquipItem = _data as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(_data, 1);

        ItemData_Equipment oldEquipItem = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDict)
        {
            if (item.Key.equipmentType == newEquipItem.equipmentType)
                oldEquipItem = item.Key;
        }
        if (oldEquipItem != null)
        {
            UnequipItem(oldEquipItem);
            AddItem(oldEquipItem);
        }
        equipment.Add(newItem);
        equipmentDict.Add(newEquipItem, newItem);
        newEquipItem.AddModifiers();
        RemoveItem(_data);

        UpdateSlotUI();
    }

    public void UnequipItem(ItemData_Equipment itemToRemove)
    {
        if (equipmentDict.TryGetValue(itemToRemove, out InventoryItem item))
        {
            equipment.Remove(item);
            equipmentDict.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();
        }
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < equipmentItem.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDict)
            {
                if (item.Key.equipmentType == equipmentItem[i].slotType)
                {
                    Debug.Log(item.Value.data.itemName);
                    equipmentItem[i].UpdataSlot(item.Value);
                }
            }
        }

        for (int i = 0; i < inventoryItem.Length; i++)
        {
            inventoryItem[i].ClearUpSlot();
        }
        for (int i = 0; i < stashItem.Length; i++)
        {
            stashItem[i].ClearUpSlot();
        }

        for (int i = 0; i < inventorys.Count; i++)
        {
            inventoryItem[i].UpdataSlot(inventorys[i]);
        }
        for (int i = 0; i < stash.Count; i++)
        {
            stashItem[i].UpdataSlot(stash[i]);
        }
        UpdataStatUI();
    }

    public void UpdataStatUI()
    {
        for (int i = 0; i < statSlot.Length; i++)
        {
            statSlot[i].UpdataStatValueUI();
        }
    }

    public void AddItem(ItemData _data)
    {
        if (_data.itemType == ItemType.Equipment && CanAddItem())
            AddInventory(_data);
        else if (_data.itemType == ItemType.Material)
            AddStash(_data);
        UpdateSlotUI();
    }

    private void AddInventory(ItemData _data)
    {
        if (inventoryDict.TryGetValue(_data, out InventoryItem item))
        {
            item.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_data, 1);
            inventorys.Add(newItem);
            inventoryDict.Add(_data, newItem);
        }
    }

    private void AddStash(ItemData _data)
    {
        if (stashDict.TryGetValue(_data, out InventoryItem item))
        {
            item.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_data, 1);
            stash.Add(newItem);
            stashDict.Add(_data, newItem);
        }
    }

    public void RemoveItem(ItemData _data)
    {
        if (inventoryDict.TryGetValue(_data, out InventoryItem item))
        {
            if (item.stackSize <= 1)
            {
                inventorys.Remove(item);
                inventoryDict.Remove(_data);
            }
            else
                item.RemoveStack();
        }
        if (stashDict.TryGetValue(_data, out InventoryItem stashItem))
        {
            if (stashItem.stackSize <= 1)
            {
                stash.Remove(stashItem);
                stashDict.Remove(_data);
                Debug.Log(_data.itemName);
            }
            else
                stashItem.RemoveStack();
        }

        UpdateSlotUI();
    }

    public bool CanAddItem()
    {
        if (inventorys.Count >= inventoryItem.Length)
            return false;
        return true;
    }
    public void UseFlask()
    {
        ItemData_Equipment currFlask = GetEquipment(EquipmentType.Flask);
        if (currFlask == null) return;
        bool canUseFlask = Time.time > lastTimeUsedFlask + flaskCooldown;

        if (canUseFlask)
        {
            if (currFlask != null)
            {
                currFlask.ItemEffect(null);
                flaskCooldown = currFlask.itemCooldown;
                lastTimeUsedFlask = Time.time;
            }
        }
        else
        {
            Debug.Log("Flask Cooldown");
        }
    }

    public bool CanUseArmor()
    {
        ItemData_Equipment currentArmor = GetEquipment(EquipmentType.Armor);
        if (Time.time > lastTimeUsedArmor + armorCooldown)
        {
            armorCooldown = currentArmor.itemCooldown;
            lastTimeUsedArmor = Time.time;
            return true;
        }
        return false;
    }

    public List<InventoryItem> GetEquipmentList() => equipment;
    public List<InventoryItem> GetStashList() => stash;
    public ItemData_Equipment GetEquipment(EquipmentType _type)
    {
        ItemData_Equipment equip = null;
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDict)
        {
            if (item.Key.equipmentType == _type)
                equip = item.Key;
        }
        return equip;
    }

    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();

        for (int i = 0; i < _requiredMaterials.Count; i++)
        {
            if (stashDict.TryGetValue(_requiredMaterials[i].data, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < _requiredMaterials[i].stackSize)
                {
                    Debug.Log("not enough materials");
                    return false;
                }
                else
                {
                    materialsToRemove.Add(stashValue);
                }
            }
            else
            {
                Debug.Log("not enough materials");
                return false;
            }
        }
        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].data);
        }
        AddItem(_itemToCraft);
        Debug.Log("Here is your item " + _itemToCraft.name);
        return true;
    }

    public void LoadData(GameData _data)
    {
        foreach(KeyValuePair<string, int> pair in _data.inventory)
        {
            foreach(var item in GetItemDataBase())
            {
                if(item != null && item.itemId == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item, pair.Value);
                    loadedItems.Add(itemToLoad);
                }
            }
        }
        foreach(string id in _data.equipmentId)
        {
            foreach(var loadItem in GetItemDataBase())
            {
                if(loadItem != null && loadItem.itemId == id)
                {
                    loadedEquipments.Add(loadItem as ItemData_Equipment);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.inventory.Clear();
        _data.equipmentId.Clear();
        foreach(KeyValuePair<ItemData, InventoryItem> item in inventoryDict)
        {
            _data.inventory.Add(item.Key.itemId, item.Value.stackSize);
        }
        foreach (KeyValuePair<ItemData, InventoryItem> item in stashDict)
        {
            _data.inventory.Add(item.Key.itemId, item.Value.stackSize);
        }
        foreach(KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDict)
        {
            _data.equipmentId.Add(item.Key.itemId);
        }
    }

    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDataBase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Scripts/Item/ItemData" });

        foreach(string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDataBase.Add(itemData);
        }

        return itemDataBase;
    }

}
