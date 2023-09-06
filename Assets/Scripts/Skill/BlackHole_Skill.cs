using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackHole_Skill : Skill
{
    [SerializeField] private UI_SkillTreeSlot blackHoleUnlockedButton;
    public bool blackHoleUnlocked { get; private set; }

    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private int attackAmount = 4;
    [SerializeField]  private float cloneAttackCooldown = 0.3f;

    [SerializeField] private float blackholeDuration;

    private BlackHole_Controller currentBlackHole;

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackHole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);
        currentBlackHole = newBlackHole.GetComponent<BlackHole_Controller>();
        currentBlackHole.SetBlackHole(maxSize, growSpeed, shrinkSpeed, attackAmount, cloneAttackCooldown, blackholeDuration);

    }

    protected override void Start()
    {
        base.Start();
        blackHoleUnlockedButton.GetComponent<Button>().onClick.AddListener(BlackHoleUnlocked);
    }

    protected override void CheckUnlock()
    {
        BlackHoleUnlocked();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void BlackHoleUnlocked()
    {
        if (blackHoleUnlockedButton.unLocked)
            blackHoleUnlocked = true;
    }

    public bool BlackHoleFinished()
    {
        if (currentBlackHole == null) return false;
        if(currentBlackHole.playerCanExitState)
        {
            currentBlackHole = null;
            return true;
        }
        return false;
    }

    public float GetBlackHoleRadius()
    {
        return maxSize / 2;
    }
}
