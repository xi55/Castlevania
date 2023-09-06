using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
{
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;

    [SerializeField] private int skillPrice;
    public bool unLocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;

    [SerializeField] private Image skillImage;
    [SerializeField] private Color lockedSkillColor;

    [SerializeField] protected UI ui;
    private void OnValidate()
    {
        gameObject.name = "SkillTressSlot - " + skillName;
    }
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }
    private void Start()
    {
        skillImage = GetComponent<Image>();
        skillImage.color = lockedSkillColor;
        ui = GetComponentInParent<UI>();
        if(unLocked)
            skillImage.color = Color.white;
    }

    public void UnlockSkillSlot()
    {
        if (PlayerManager.instance.HaveEnoughtMoney(skillPrice) == false) return;
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unLocked == false)
            {
                Debug.Log("Connot unlock skill");
                return;
            }
        }

        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unLocked == true)
            {
                Debug.Log("Connot lock skill");
                return;
            }
        }

        unLocked = true;
        skillImage.color = Color.white;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillTip.ShowToolTip(skillName, skillDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillTip.HideToolTip();
    }

    public void LoadData(GameData _data)
    {
        if(_data.skillTree.TryGetValue(skillName, out bool value))
        {
            unLocked = value;
        }
    }

    public void SaveData(ref GameData _data)
    {
        //throw new System.NotImplementedException();
        if(_data.skillTree.TryGetValue(skillName, out bool value))
        {
            _data.skillTree.Remove(skillName);
            _data.skillTree.Add(skillName, unLocked);
        }
        else
            _data.skillTree.Add(skillName, unLocked);
    }
}
