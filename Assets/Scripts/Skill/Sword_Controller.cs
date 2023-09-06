using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Controller : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CircleCollider2D circleCollider;
    private Player player;

    private bool canRotate = true;
    private bool isReturn = false;
    [SerializeField] private float returnSpeed = 12f;

    [Header("Pierce info")]
    [SerializeField] private int pierceAmount;
    [SerializeField] private bool isPierce;

    [Header("Bouncing info")]
    [SerializeField] private float bounceSpeed;
    private bool isBouncing;
    private int BouncingIndex;
    [SerializeField] private int bounceAmount;
    [SerializeField] private List<Transform> enemyTarget;

    [Header("Spin info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;
    private float hitCooldown;
    private float hitTimer;
    private float spinDirection;

    private float freezeTime;


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;

        if (isReturn)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 1f)
                player.ClearSword();
        }

        

        BounceLogic();

        SpinLogic();

    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(transform.position, player.transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }
            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(spinDirection + transform.position.x, spinDirection + transform.position.y), 1.5f * Time.deltaTime);

                if (spinTimer < 0)
                {
                    isReturn = true;
                    isSpinning = false;
                }
                hitTimer -= Time.deltaTime;
                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
                    for (int i = 0; i < colliders.Length; i++)
                    {
                        if (colliders[i].gameObject.GetComponent<Enemy>() != null)
                        {
                            //colliders[i].gameObject.GetComponent<Enemy>().Damage();
                            EnemyStarts enemy = colliders[i].gameObject.GetComponent<EnemyStarts>();
                            PlayerManager.instance.player.states.DoDamage(enemy);
                        }
                            
                    }
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        spinTimer = spinDuration;
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[BouncingIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[BouncingIndex].position) < .1f)
            {
                SkillFreezeTime(enemyTarget[BouncingIndex].GetComponent<Enemy>());
                BouncingIndex++;
                bounceAmount--;
                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturn = true;
                }
                if (BouncingIndex >= enemyTarget.Count)
                    BouncingIndex = 0;

            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isReturn) return;

        if(collision.gameObject.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            SkillFreezeTime(enemy);
        }
        

        
        SetupTargetForBounce(collision);
        StuckInto(collision);

    }

    private void SkillFreezeTime(Enemy enemy)
    {
        //enemy.Damage();
        player.states.DoDamage(enemy.GetComponent<CharacterStarts>());
        if(player.skill.swordSkill.timeStopUnlock)
            enemy.StartCoroutine(enemy.FreezeTimeFor(freezeTime));
        if (player.skill.swordSkill.vulnerabilityUnlock)
            enemy.states.MakeVulnerableFor(freezeTime);

        ItemData_Equipment equipment = Inventory.instance.GetEquipment(EquipmentType.Amulet);
        if (equipment != null)
            equipment.ItemEffect(enemy.transform);

    }

    private void SetupTargetForBounce(Collision2D collision)
    {
        //collision.gameObject.GetComponent<Enemy>()?.Damage();
       

        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            player.states.DoDamage(collision.gameObject.GetComponent<CharacterStarts>());
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10f);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject.GetComponent<Enemy>() != null)
                        enemyTarget.Add(colliders[i].transform);
                }
            }
        }
    }

    public void SetupBounce(bool _isBouncing, int _amountOfBounce)
    {
        isBouncing = _isBouncing;
        bounceAmount = _amountOfBounce;

        enemyTarget = new List<Transform>();
    }

    public void SetupPierce(bool _isPierce, int _pierceAmount)
    {
        isPierce = _isPierce;
        pierceAmount = _pierceAmount;
    }

    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        spinDuration = _spinDuration;
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        hitCooldown = _hitCooldown;
    }

    private void StuckInto(Collision2D collision)
    {
        if (isSpinning)
        {
            Physics2D.IgnoreCollision(circleCollider, collision.gameObject.GetComponent<Collider2D>());
            StopWhenSpinning();
            return;
        }

        if (pierceAmount > 0 && collision.gameObject.GetComponent<Enemy>() != null)
        {
            Physics2D.IgnoreCollision(circleCollider, collision.gameObject.GetComponent<Collider2D>());
            pierceAmount--;
            return;
        }


        canRotate = false;
        circleCollider.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count >0) return;
        animator.SetBool("rotation", false);
        transform.parent = collision.transform;
    }

    public void ReturnSword()
    {
        animator.SetBool("rotation", true);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;
        transform.parent = null;
        isReturn = true;
    }

    public void SetupSword(Vector2 dir, float swordGravity, Player _player, float _freezeTime)
    {
        rb.velocity = dir;
        rb.gravityScale = swordGravity;
        player = _player;
        freezeTime = _freezeTime;
        if (pierceAmount <= 0)
            animator.SetBool("rotation", true);
        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

        Invoke("DestroyMe", 7f);

    }
}
