using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill
{
    [Header("parry")]
    [SerializeField] private UI_SkillTreeSlot parryUnlockedButton;
    public bool parryUnlocked { get; private set; }

    [Header("parry restore")]
    [SerializeField] private UI_SkillTreeSlot restoreUnlockedButton;
    [Range(0f, 1f)]
    [SerializeField] private float restorePercent;
    public bool restoreUnlocked { get; private set; }

    [Header("Parry with mirage")]
    [SerializeField] private UI_SkillTreeSlot parryWithMirageUnlockedButton;
    public bool parryWithMirage { get; private set; }

    public override void UseSkill()
    {
        base.UseSkill();
        if(restoreUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(restorePercent * player.states.GetMaxHealthValue());
            player.states.IncreaseHealth(restoreAmount);
        }
    }

    protected override void Start()
    {
        base.Start();
        parryUnlockedButton.GetComponent<Button>()?.onClick.AddListener(UnlockedParry);
        restoreUnlockedButton.GetComponent<Button>()?.onClick.AddListener(UnlockedParryRestore);
        parryWithMirageUnlockedButton.GetComponent<Button>()?.onClick.AddListener(UnlockedParryWithMirage);
    }

    protected override void CheckUnlock()
    {
        UnlockedParry();
        UnlockedParryRestore();
        UnlockedParryWithMirage();
    }
    public void UnlockedParry()
    {
        if (parryUnlockedButton.unLocked)
            parryUnlocked = true;
    }

    public void UnlockedParryRestore()
    {
        if (restoreUnlockedButton.unLocked)
            restoreUnlocked = true;
    }

    public void UnlockedParryWithMirage()
    {
        if (parryWithMirageUnlockedButton.unLocked)
            parryWithMirage = true;
    }

    public void MakeParryWithMirage(Transform _responTrans)
    {
        if (parryWithMirage)
            SkillManager.instance.cloneSkill.CreateCloneOnCounterAttack(_responTrans);
    }

}
