using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Start
{
    [SerializeField] private int baseValue;
    public List<int> modifiers;
    public int GetValue()
    {
        int finalValue = baseValue;

        foreach (var modifier in modifiers)
            finalValue += modifier;

        return finalValue;
    }

    public void SetDefultValue(int _value)
    {
        baseValue = _value;
    }

    public void AddModifiers(int _modifier)
    {
        modifiers.Add(_modifier);
    }

    public void RemoveModifiers(int _modifier)
    {
        modifiers.Remove(_modifier);
    }
}
