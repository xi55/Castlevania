using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New item data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;
    public float itemCooldown;
    public ItemEffect[] itemEffects;

    [TextArea]
    public string itemEffectsDescription;

    [Header("Major States")]
    public int strength;
    public int agility;
    public int intelligence;
    public int vitality;

    [Header("Offensive States")]
    public int damage;
    public int critChance;
    public int critPower;


    [Header("Defensive States")]
    public int health;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("Magic States")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials;

    public void ItemEffect(Transform _enemyPos)
    {
        foreach (var effect in itemEffects)
        {
            effect.ExecuteEffect(_enemyPos);
        }
    }
    public void AddModifiers()
    {
        PlayerStarts playerStarts = PlayerManager.instance.player.GetComponent<PlayerStarts>();

        playerStarts.strength.AddModifiers(strength);
        playerStarts.agility.AddModifiers(agility);
        playerStarts.intelligence.AddModifiers(intelligence);
        playerStarts.vitality.AddModifiers(vitality);

        playerStarts.damage.AddModifiers(damage);
        playerStarts.critChance.AddModifiers(critChance);
        playerStarts.critPower.AddModifiers(critPower);

        playerStarts.maxHealth.AddModifiers(health);
        playerStarts.armor.AddModifiers(armor);
        playerStarts.evasion.AddModifiers(evasion);
        playerStarts.magicResistance.AddModifiers(magicResistance);

        playerStarts.fireDamage.AddModifiers(fireDamage);
        playerStarts.iceDamage.AddModifiers(iceDamage);
        playerStarts.lightingDamage.AddModifiers(lightingDamage);

    }

    public void RemoveModifiers()
    {
        PlayerStarts playerStarts = PlayerManager.instance.player.GetComponent<PlayerStarts>();

        playerStarts.strength.RemoveModifiers(strength);
        playerStarts.agility.RemoveModifiers(agility);
        playerStarts.intelligence.RemoveModifiers(intelligence);
        playerStarts.vitality.RemoveModifiers(vitality);

        playerStarts.damage.RemoveModifiers(damage);
        playerStarts.critChance.RemoveModifiers(critChance);
        playerStarts.critPower.RemoveModifiers(critPower);

        playerStarts.maxHealth.RemoveModifiers(health);
        playerStarts.armor.RemoveModifiers(armor);
        playerStarts.evasion.RemoveModifiers(evasion);
        playerStarts.magicResistance.RemoveModifiers(magicResistance);

        playerStarts.fireDamage.RemoveModifiers(fireDamage);
        playerStarts.iceDamage.RemoveModifiers(iceDamage);
        playerStarts.lightingDamage.RemoveModifiers(lightingDamage);
    }

    public override string GetDescription()
    {
        sb.Length = 0;

        AddItemDescription(strength, "Strength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intelligence, "Intelligence");
        AddItemDescription(vitality, "Vitality");

        AddItemDescription(damage, "Damage");
        AddItemDescription(critChance, "CritChance");
        AddItemDescription(critPower, "CritPower");

        AddItemDescription(health, "Health");
        AddItemDescription(armor, "Armor");
        AddItemDescription(evasion, "Evasion");
        AddItemDescription(magicResistance, "MagicResist");

        AddItemDescription(fireDamage, "FireDMG");
        AddItemDescription(iceDamage, "IceDMG");
        AddItemDescription(lightingDamage, "LightDMG");
        Debug.Log(itemEffectsDescription);
        if(itemEffectsDescription.Length > 0)
        {
            sb.AppendLine();
            sb.Append(itemEffectsDescription);
        }

        return sb.ToString();
    }

    private void AddItemDescription(int _value, string _name)
    {
        if(_value != 0)
        {
            if (sb.Length > 0)
                sb.AppendLine();
            if(_value > 0)
                sb.Append("+ " +  _name + ": " + _value);
        }
    }

}

