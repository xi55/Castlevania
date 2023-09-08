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

    public SerializableDict<string, bool> checkpoint;
    public string closestCheckpointId;

    public int lostCurrencyAmount;
    public float lostCurrencyX;
    public float lostCurrencyY;

    public SerializableDict<string, float> volumeSetting;
    public GameData()
    {
        currency = 0;
        inventory = new SerializableDict<string, int>();
        equipmentId = new List<string>();
        skillTree = new SerializableDict<string, bool>();

        checkpoint = new SerializableDict<string, bool>();
        closestCheckpointId = string.Empty;

        lostCurrencyAmount = 0;
        lostCurrencyX = 0;
        lostCurrencyY = 0;

        volumeSetting = new SerializableDict<string, float>();
    }
}
