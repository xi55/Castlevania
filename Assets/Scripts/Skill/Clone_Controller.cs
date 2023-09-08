using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Controller : MonoBehaviour
{

    [SerializeField] private float cloneLossingSpeed;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackRedius;
    [SerializeField] private Animator animator;

    private float chanceToDuplicate;
    private float attackMultipler;
     private Transform closeEnemy;
    private float cloneTimer;
    private SpriteRenderer sr;

    private int faceDir = 1;
    private bool canDuplicateClone;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if(cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * cloneLossingSpeed));
            if (sr.color.a <= 0)
                Destroy(gameObject);
        }
    }

    public void SetupClone(Transform _transform, float cloneDuration, bool canAttack, Vector3 _offset, Transform _closeEnemy, bool _canDuplicateClone, float _chanceToDuplicate, float _attackMultipler)
    {
        if (canAttack)
            animator.SetInteger("attackNum", Random.Range(1, 3));
        transform.position = _transform.position + _offset;
        cloneTimer = cloneDuration;
        closeEnemy = _closeEnemy;
        canDuplicateClone = _canDuplicateClone;
        chanceToDuplicate = _chanceToDuplicate;
        attackMultipler = _attackMultipler;
        //Debug.Log(transform.position);
        FaceToTarget();
    }


    private void AnimationCalled()
    {
        cloneTimer = -0.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackRedius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Character>().SetupKonckBackDir(transform);
                Player player = PlayerManager.instance.player;
                PlayerStarts playerStarts = player.GetComponent<PlayerStarts>();
                EnemyStarts enemyStarts = hit.GetComponent<EnemyStarts>();
                playerStarts.CloneDoDamage(enemyStarts, attackMultipler);

                if(player.skill.cloneSkill.canApplyOnHitEffect)
                {
                    ItemData_Equipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);
                    if (weaponData != null)
                        weaponData.ItemEffect(hit.transform);
                }

                if (canDuplicateClone)
                {
                    if(Random.Range(0, 100) < chanceToDuplicate)
                        SkillManager.instance.cloneSkill.CreateClone(hit.transform, new Vector3(1.5f * faceDir, 0));
                }
            }
        }
    }

    private void FaceToTarget()
    {
        
        if (closeEnemy != null)
        {
            if(transform.position.x > closeEnemy.position.x)
            {
                faceDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
