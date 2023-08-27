using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;

    [SerializeField] private bool creatCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashOver;
    [SerializeField] private bool canCreateCloneOnCounterAttack;
    [SerializeField] private bool canDuplicateClone;

    [Header("Crystal instead of clone")]
    public bool crystalInseadOfClone;

    [SerializeField] private Clone_Controller currentClone;
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
        currentClone.SetupClone(clonePosition, cloneDuration, canAttack, _offset, GetCloseTarget(clonePosition), canDuplicateClone);
    }

    public void CreateCloneOnDashStart()
    {
        if(creatCloneOnDashStart)
            CreateClone(player.transform, Vector3.zero);
    }

    public void CreateCloneOnDashOver()
    {
        if (createCloneOnDashOver)
            CreateClone(player.transform, Vector3.zero);
    }

    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        if (canCreateCloneOnCounterAttack)
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(2 * player.faceDir, 0)));
    }

    private IEnumerator CreateCloneWithDelay(Transform _trasnform, Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_trasnform, _offset);
    }

}
