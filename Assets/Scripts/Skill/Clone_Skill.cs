using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clone_Skill : Skill
{
    [Header("Clone info")]
    [SerializeField] private float attackMultipler;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;

    [Space]
    [Header("Clone Attack")]
    [SerializeField] private bool canAttack;
    [SerializeField] private float cloneAttackMultipler;
    [SerializeField] private UI_SkillTreeSlot cloneAttackUnlockedButton;

    [Space]
    [Header("Aggresive Attack")]
    [SerializeField] private UI_SkillTreeSlot aggresiveMirageUnlockedButton;
    [SerializeField] private float aggresiveMirageAttackMultipler;
    public bool canApplyOnHitEffect { get; private set; }

    [Space]
    [Header("Multiple Clone")]
    [SerializeField] private UI_SkillTreeSlot multipleCloneUnlockedButton;
    [SerializeField] private float multipleCloneAttackMultipler;
    [SerializeField] private float chanceToDuplicate;
    [SerializeField] private bool canDuplicateClone;
    [Space]
    [Header("Crystal instead of clone")]
    [SerializeField] private UI_SkillTreeSlot crystalInsteadUnlockedButton;
    public bool crystalInseadOfClone;

    [SerializeField] private Clone_Controller currentClone;

    protected override void Start()
    {
        base.Start();
        cloneAttackUnlockedButton.GetComponent<Button>().onClick.AddListener(CloneAttackUnlocked);
        aggresiveMirageUnlockedButton.GetComponent<Button>().onClick.AddListener(AggresiveMirageUnlocked);
        multipleCloneUnlockedButton.GetComponent <Button>().onClick.AddListener(MultipleCloneUnlocked);
        crystalInsteadUnlockedButton.GetComponent<Button>().onClick.AddListener(CrystalInsteadUnlocked);
    }

    protected override void CheckUnlock()
    {
        CloneAttackUnlocked();
        AggresiveMirageUnlocked();
        MultipleCloneUnlocked();
        CrystalInsteadUnlocked();
    }
    public void CreateClone(Transform clonePosition, Vector3 _offset)
    {
        if (crystalInseadOfClone)
        {
            SkillManager.instance.crystalSkill.CreateCrystal();
            return;
        }
        GameObject newclone  = Instantiate(clonePrefab);
        currentClone = newclone.GetComponent<Clone_Controller>();
        //Debug.Log(currentClone.transform.position);
        currentClone.SetupClone(clonePosition, cloneDuration, canAttack, _offset, GetCloseTarget(clonePosition), canDuplicateClone, chanceToDuplicate, attackMultipler);
    }

    #region Unlocked region

    private void CloneAttackUnlocked()
    {
        if(cloneAttackUnlockedButton.unLocked)
        {
            attackMultipler = cloneAttackMultipler;
            canAttack = true;
        }
    }

    private void AggresiveMirageUnlocked()
    {
        if(aggresiveMirageUnlockedButton.unLocked)
        {
            attackMultipler = aggresiveMirageAttackMultipler;
            canApplyOnHitEffect = true;
        }
    }

    private void MultipleCloneUnlocked()
    {
        if(multipleCloneUnlockedButton.unLocked)
        {
            attackMultipler = multipleCloneAttackMultipler;
            canDuplicateClone = true;
        }
    }

    private void CrystalInsteadUnlocked()
    {
        if(crystalInsteadUnlockedButton.unLocked)
        {
            crystalInseadOfClone = true;
        }
    }

    #endregion

    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(2 * player.faceDir, 0)));
    }

    private IEnumerator CreateCloneWithDelay(Transform _trasnform, Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_trasnform, _offset);
    }

}
