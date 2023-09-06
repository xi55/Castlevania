using UnityEngine.EventSystems;
using UnityEngine;
public class UI_CraftSlot : UI_ItemSlot
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public void SetupCraftSlot(ItemData_Equipment _data)
    {
        if (_data == null) return;

        item.data = _data;
        itemImage.sprite = _data.icon;
        itemText.text = _data.name;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.data == null) return;
        
        ui.craftWindow.SetupCraftWindow(item.data as ItemData_Equipment);
    }
}
