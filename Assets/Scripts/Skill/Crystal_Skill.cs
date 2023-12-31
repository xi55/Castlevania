using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crystal_Skill : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalDuration;
    private Crystal_Controller currentCrystal;

    

    [Header("Crystal Move")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool canMove;
    [SerializeField] private UI_SkillTreeSlot crystalMoveUnlockedButton;

    [Header("Crystal Explosive")]
    [SerializeField] private bool canBoom;
    [SerializeField] private UI_SkillTreeSlot crystalExplosiveUnlockedButton;

    [Header("Muilt Crystal")]
    [SerializeField] private float useWindowTime;
    [SerializeField] private bool canMulitCrystal;
    [SerializeField] private int crystalAmount;
    [SerializeField] private float muiltCrystalCooldown;
    [SerializeField] private List<GameObject> crystals = new List<GameObject>();
    [SerializeField] private UI_SkillTreeSlot crystalMuiltUnlockedButton;


    [Header("Crystal")]
    [SerializeField] private UI_SkillTreeSlot crystalUnlockedButton;
    public bool crystalUnlocked { get; private set; }

    [Header("Crystal mirage")]
    [SerializeField] private bool cloneInsteadOfCrystal;
    [SerializeField] private UI_SkillTreeSlot crystalMirageUnlockedButton;

    protected override void Start()
    {
        base.Start();
        crystalUnlockedButton.GetComponent<Button>().onClick.AddListener(CrystalUnlocked);
        crystalMoveUnlockedButton.GetComponent<Button>().onClick.AddListener(CrystalMoveUnlocked);
        crystalExplosiveUnlockedButton.GetComponent<Button>().onClick.AddListener(CrystalExplosiveUnlocked);
        crystalMuiltUnlockedButton.GetComponent<Button>().onClick.AddListener(CrystalMuiltUnlocked);
        crystalMirageUnlockedButton.GetComponent<Button>().onClick.AddListener(CrystalMirageUnlocked);
    }

    protected override void CheckUnlock()
    {
        CrystalUnlocked();
        CrystalMoveUnlocked();
        CrystalExplosiveUnlocked();
        CrystalMuiltUnlocked();
        CrystalMirageUnlocked();
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMuiltCrystal()) return;

        if(currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMove) return;
            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if(cloneInsteadOfCrystal)
            {
                SkillManager.instance.cloneSkill.CreateClone(currentCrystal.transform, Vector3.zero);
                currentCrystal.DestroyMe();
            }
            else
            {
                currentCrystal?.FinishCrystal();
            }

        }
    }

    public void CreateCrystal()
    {
        GameObject newCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        currentCrystal = newCrystal.GetComponent<Crystal_Controller>();
        currentCrystal.SetupCrystal(crystalDuration, moveSpeed, canBoom, canMove, GetCloseTarget(currentCrystal.transform));
    }

    public void ChooseRandomTarget() => currentCrystal.ChooseRandomEnemy();

    private bool CanUseMuiltCrystal()
    {
        if(canMulitCrystal)
        {
            if(crystals.Count > 0)
            {
                if (crystals.Count == crystalAmount)
                    Invoke("ResetAbility", useWindowTime);

                cooldown = 0;
                GameObject crystalToSpwn = crystals[crystals.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpwn, player.transform.position, Quaternion.identity);
                crystals.Remove(crystalToSpwn);
                newCrystal.GetComponent<Crystal_Controller>().SetupCrystal(crystalDuration, moveSpeed, canBoom, canMove, GetCloseTarget(newCrystal.transform));
                if(crystals.Count <= 0)
                {
                    cooldown = muiltCrystalCooldown;
                    RefilCrystal();
                }
            }
            return true;
        }
        return false;
    }

    private void RefilCrystal()
    {
        int crystalNum = crystalAmount - crystals.Count;
        for (int i = 0; i < crystalNum; i++)
        {
            crystals.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (cooldown > 0) return;
        cooldownTimer = muiltCrystalCooldown;
        RefilCrystal();
    }

    #region unlock skill region
    public void CrystalMoveUnlocked()
    {
        if (crystalMoveUnlockedButton.unLocked)
            canMove = true;
    }

    public void CrystalExplosiveUnlocked()
    {
        if (crystalExplosiveUnlockedButton.unLocked)
            canBoom = true;
    }
    public void CrystalMuiltUnlocked()
    {
        if (crystalMuiltUnlockedButton.unLocked)
            canMulitCrystal = true;
    }
    public void CrystalUnlocked()
    {
        if (crystalUnlockedButton.unLocked)
            crystalUnlocked = true;
    }
    public void CrystalMirageUnlocked()
    {
        if (crystalMirageUnlockedButton.unLocked)
            cloneInsteadOfCrystal = true;
    }
    #endregion
}
