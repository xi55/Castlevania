using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image[] materialImage;

    [SerializeField] private Button craftButton;


    public void SetupCraftWindow(ItemData_Equipment _item)
    {
        //Debug.Log(_item);
        craftButton.onClick.RemoveAllListeners();

        for(int i = 0; i < materialImage.Length; i++)
        {
            materialImage[i].color = Color.clear;
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }
        for(int i = 0; i < _item.craftingMaterials.Count; i++)
        {
            if (_item.craftingMaterials.Count > materialImage.Length)
                return;
            materialImage[i].sprite = _item.craftingMaterials[i].data.icon;
            materialImage[i].color = Color.white;

            TextMeshProUGUI materialSlotText = materialImage[i].GetComponentInChildren<TextMeshProUGUI>();
            materialSlotText.color = Color.white;
            materialSlotText.text = _item.craftingMaterials[i].stackSize.ToString();

        }
        itemIcon.sprite = _item.icon;
        itemName.text = _item.itemName;
        itemDescription.text = _item.GetDescription();
        craftButton.onClick.AddListener(() => Inventory.instance.CanCraft(_item, _item.craftingMaterials));
    }


}
