using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;

    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;
    [TextArea]
    [SerializeField] private string statDescription;

    private void OnValidate()
    {
        statName = statType.ToString();
        gameObject.name = "Stat - " + statName;
        if(statNameText != null)
            statNameText.text =statName + ": ";
    }

    void Start()
    {
        ui = GetComponentInParent<UI>();
        UpdataStatValueUI();
    }

    public void UpdataStatValueUI()
    {
        PlayerStarts playerStarts = PlayerManager.instance.player.GetComponent<PlayerStarts>();
        if(playerStarts != null)
        {
            statValueText.text = playerStarts.GetStat(statType).GetValue().ToString();

            if(statType == StatType.health)
                statValueText.text = playerStarts.GetMaxHealthValue().ToString();
            if(statType == StatType.damage)
                statValueText.text = (playerStarts.damage.GetValue() + playerStarts.strength.GetValue()).ToString();
            if (statType == StatType.critPower)
                statValueText.text = (playerStarts.critPower.GetValue() + playerStarts.strength.GetValue()).ToString();
            if (statType == StatType.critChance)
                statValueText.text = (playerStarts.critChance.GetValue() + playerStarts.agility.GetValue()).ToString();
            if (statType == StatType.evasion)
                statValueText.text = (playerStarts.evasion.GetValue() + playerStarts.agility.GetValue()).ToString();
            if(statType == StatType.magicResistance)
                statValueText.text = (playerStarts.magicResistance.GetValue() + (playerStarts.intelligence.GetValue() * 3)).ToString();

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statTip.ShowStatToolTip(statDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statTip.HideStatToolTip();
    }
}
