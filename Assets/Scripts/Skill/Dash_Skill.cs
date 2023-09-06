using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("Dash")]
    [SerializeField] private UI_SkillTreeSlot dashUnlockedButton;
    public bool dashUnlocked { get; private set; }
    

    [Header("Clone On Dash")]
    [SerializeField] private UI_SkillTreeSlot cloneOndashUnlockedButton;
    public bool cloneOndashUnlocked { get; private set; }
    

    [Header("Clone On arrival")]
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalButton;
    public bool cloneOnArrivalUnlocked { get; private set; }
    


    protected override void Update()
    {
        base.Update();
    }

    public override void UseSkill()
    {
        base.UseSkill();
    }
    protected override void Start()
    {
        base.Start();
        dashUnlockedButton.GetComponent<Button>()?.onClick.AddListener(UnlockedDash);
        cloneOndashUnlockedButton.GetComponent<Button>()?.onClick.AddListener(UnlockedCloneOnDash);
        cloneOnArrivalButton.GetComponent<Button>()?.onClick.AddListener(UnlockedCloneOnArrival);
    }

    protected override void CheckUnlock()
    {
        UnlockedDash();
        UnlockedCloneOnDash();
        UnlockedCloneOnArrival();
    }
    private void UnlockedDash()
    {
        if (dashUnlockedButton.unLocked)
        {
            dashUnlocked = true;
        }
    }

    private void UnlockedCloneOnDash()
    {
        if (cloneOndashUnlockedButton.unLocked)
        {
            cloneOndashUnlocked = true;
        }
    }

    private void UnlockedCloneOnArrival()
    {
        if (cloneOnArrivalButton.unLocked)
            cloneOnArrivalUnlocked = true;
    }

    public void CloneOnDash()
    {
        if (cloneOndashUnlocked)
            SkillManager.instance.cloneSkill.CreateClone(player.transform, Vector3.zero);
    }

    public void CloneOnArrival()
    {
        if (cloneOnArrivalUnlocked)
            SkillManager.instance.cloneSkill.CreateClone(player.transform, Vector3.zero);
    }


}
