using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill
{
    [Header("Dodge")]
    [SerializeField ] private int evasionAmount;
    [SerializeField] private UI_SkillTreeSlot dodgeUnlockedButton;
    public bool dodgeUnlocked { get; private set; }

    [Header("Dodge Mirage")]
    [SerializeField] private UI_SkillTreeSlot dodgeMirageUnlockedButton;
    public bool mirageUnlocked { get; private set; }

    public override void UseSkill()
    {
        base.UseSkill();
    }

    protected override void Start()
    {
        base.Start();
        dodgeUnlockedButton.GetComponent<Button>().onClick.AddListener(DodgeUnlocked);
        dodgeMirageUnlockedButton.GetComponent<Button>().onClick.AddListener(DodgeMirageUnlocked);
    }

    protected override void CheckUnlock()
    {
        DodgeUnlocked();
        DodgeMirageUnlocked();
    }

    public void DodgeUnlocked()
    {
        if (dodgeUnlockedButton.unLocked)
        {
            PlayerStarts playerStarts = player.GetComponent<PlayerStarts>();
            playerStarts.evasion.AddModifiers(evasionAmount);
            //player.states.evasion.AddModifiers(evasionAmount);
            Inventory.instance.UpdataStatUI();
            dodgeUnlocked = true;
        }
    }

    public void DodgeMirageUnlocked()
    {
        if(dodgeMirageUnlockedButton.unLocked)
            mirageUnlocked = true;
    }

    public void CreateMirageOnDodge()
    {
        if (mirageUnlocked)
            SkillManager.instance.cloneSkill.CreateClone(player.transform, new Vector3(player.faceDir * 2, 0));
    }

}
