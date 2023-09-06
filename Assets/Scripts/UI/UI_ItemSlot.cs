using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    public InventoryItem item;
    [SerializeField] protected UI ui;

    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    public void UpdataSlot(InventoryItem _item)
    {

        item = _item;
        //Debug.Log(item.data.itemName);
        if (item != null)
        {
            itemImage.color = Color.white;
            itemImage.sprite = item.data.icon;
            if (item.stackSize > 1)
                itemText.text = item.stackSize.ToString();
            else
                itemText.text = "";
        }
    }

    public void ClearUpSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null) return;
        if(Input.GetKey(KeyCode.LeftAlt) && item != null)
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }

        //Debug.Log("Equiped new item " + item.data.itemName);
        if (item.data.itemType == ItemType.Equipment)
            Inventory.instance.EquipItem(item.data);

        ui.itemTip.HideToolTip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null || item.data == null) return;

        Vector2 mousePos = Input.mousePosition;
        float xOffset = 0;
        float yOffset = 0;

        if (mousePos.x > 600)
            xOffset = -200;
        else
            xOffset = 200;

        if (mousePos.y > 320)
            yOffset = -170;
        else
            yOffset = 170;

        ui.itemTip.ShowToolTip(item.data as ItemData_Equipment);
        ui.itemTip.transform.position = new Vector2(mousePos.x + xOffset, mousePos.y + yOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null || item.data == null) return;
        ui.itemTip.HideToolTip();
    }
}
