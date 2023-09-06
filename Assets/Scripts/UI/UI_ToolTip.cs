using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UI_ToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;

    public void ShowToolTip(ItemData_Equipment _Equipment)
    {
        itemNameText.text = _Equipment.itemName;
        itemTypeText.text = _Equipment.equipmentType.ToString();
        itemDescriptionText.text = _Equipment.GetDescription();

        gameObject.SetActive(true);
    }

    public void HideToolTip() => gameObject.SetActive(false);
}
