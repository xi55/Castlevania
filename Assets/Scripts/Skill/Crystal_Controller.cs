using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Controller : MonoBehaviour
{
    private Animator animator => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();

    private float crystalExitTimer;

    private float moveSpeed;
    private bool canBoom;
    private bool canMove;

    private float growSpeed = 5;
    private bool canGrow;

    private Transform closeEnemy;
    [SerializeField] private LayerMask enemy;

    public void SetupCrystal(float _crystalDuration, float _moveSpeed, bool _canBoom, bool _canMove, Transform _closeEnemy)
    {
        crystalExitTimer = _crystalDuration;
        moveSpeed = _moveSpeed;
        canBoom = _canBoom;
        canMove = _canMove;
        closeEnemy = _closeEnemy;
    }

    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackHoleSkill.GetBlackHoleRadius();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, enemy);
        if(colliders.Length > 0)
            closeEnemy = colliders[Random.Range(0, colliders.Length)].transform;
    }

    private void Update()
    {
        crystalExitTimer -= Time.deltaTime;
        if (crystalExitTimer < 0)
        {
            FinishCrystal();
        }
        if(canMove && closeEnemy != null)
        {
            transform.position = Vector2.Lerp(transform.position, closeEnemy.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, closeEnemy.position) < 1f)
                FinishCrystal();
        }
        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(1, 1), growSpeed * Time.deltaTime);

    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                //hit.GetComponent<Enemy>().Damage();
                PlayerManager.instance.player.states.DoMagicDamage(hit.GetComponent<CharacterStarts>());

                ItemData_Equipment equipment = Inventory.instance.GetEquipment(EquipmentType.Amulet);
                if (equipment != null)
                    equipment.ItemEffect(hit.transform);
            }
        }
    }

    public void FinishCrystal()
    {
        if (canBoom)
        {
            canGrow = true;
            animator.SetTrigger("boom");
        }
        else
            DestroyMe();
    }

    public void DestroyMe() => Destroy(gameObject);
}
