using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeSave : MonoBehaviour, ISaveManager
{
    [SerializeField] private Transform skillSlotParent;
    private UI_SkillTreeSlot[] skillSlot;

    private void Awake()
    {
        skillSlot = skillSlotParent.GetComponentsInChildren<UI_SkillTreeSlot>();
    }
    public void LoadData(GameData _data)
    {
        for(int i = 0; i < skillSlot.Length; i++)
        {
            if (_data.skillTree.TryGetValue(skillSlot[i].skillName, out bool value))
            {
                skillSlot[i].unLocked = value;
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        for (int i = 0; i < skillSlot.Length; i++)
        {
            if(_data.skillTree.TryGetValue(skillSlot[i].skillName, out bool value))
            {
                _data.skillTree.Remove(skillSlot[i].skillName);
                _data.skillTree.Add(skillSlot[i].skillName, skillSlot[i].unLocked);
            }
            else
                _data.skillTree.Add(skillSlot[i].skillName, skillSlot[i].unLocked);
        }
    }
}
