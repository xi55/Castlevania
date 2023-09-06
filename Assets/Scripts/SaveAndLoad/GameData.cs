using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;
    public SerializableDict<string, int> inventory;
    public List<string> equipmentId;
    public SerializableDict<string, bool> skillTree;
    public GameData()
    {
        currency = 0;
        inventory = new SerializableDict<string, int>();
        equipmentId = new List<string>();
        skillTree = new SerializableDict<string, bool>();
    }
}
