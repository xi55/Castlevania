using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    public Collider2D col { get; private set; }

    public SpriteRenderer sr { get; private set; }

    public CharacterStarts states { get; private set; }
    public Animator animator { get; private set; }
    public int faceDir { get; private set; } = 1;
    protected bool faceRight = true;
    public bool isBusy { get; private set; }

    [Header("HitBack info")]
    [SerializeField] protected Vector2 hitBackDis;
    [SerializeField] protected bool isHitBack;

    [Header("Physics Check")]
    [SerializeField] protected LayerMask isGround;
    [SerializeField] protected LayerMask isWall;
    [SerializeField] protected Vector2 bottomOffset, checkOffset;
    public Transform attackCheck;
    public float attackRedius;

    public int konckbackDir { get; private set; }

    [HideInInspector]public CharacterFX FX;

    public System.Action onFlipped;
    protected virtual void Awake()
    {

    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        FX = GetComponent<CharacterFX>();
        states = GetComponentInChildren<CharacterStarts>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public virtual void SlowCharacter(float _slowPercentage, float _slowDuration)
    {

    }

    protected virtual void ReturnDefultSpeed()
    {
        animator.speed = 1;
    }

    public void Flip()
    {
        faceDir *= -1;
        faceRight = !faceRight;
        transform.Rotate(0, 180, 0);
        if(onFlipped != null)
            onFlipped();
    }

    public IEnumerator IsBusy(float seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(seconds);
        isBusy = false;
    }

    public void FlipController(float xVelocity)
    {
        if (xVelocity > 0 && !faceRight)
            Flip();
        else if (xVelocity < 0 && faceRight)
            Flip();
    }

    public IEnumerator HitBack()
    {
        isHitBack = true;
        rb.velocity = new Vector2(hitBackDis.x * konckbackDir, hitBackDis.y);
        yield return new WaitForSeconds(0.07f);
        isHitBack=false;
    }

    public virtual void SetupKonckBackDir(Transform _damage)
    {
        if (_damage.position.x > transform.position.x)
            konckbackDir = -1;
        else
            konckbackDir = 1;
    }

    public bool IsGroundDetect() => Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset * new Vector2(faceDir, 1), .15f, isGround);
    public bool IsWallDetect() => Physics2D.OverlapCircle((Vector2)transform.position + checkOffset * new Vector2(faceDir, 1), .15f, isWall);
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isHitBack) return;
        rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    public virtual void DamageEffect() => StartCoroutine(HitBack());



    public virtual void Die()
    {

    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset * new Vector2(faceDir, 1), 0.15f);
        Gizmos.DrawWireSphere((Vector2)transform.position + checkOffset * new Vector2(faceDir, 1), 0.15f);

        Gizmos.DrawSphere(attackCheck.position, attackRedius);
    }

}
