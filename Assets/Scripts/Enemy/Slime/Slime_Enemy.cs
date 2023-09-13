using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlimeType { big, medium, small}

public class Slime_Enemy : Enemy
{
    [Header("Slime Spesific")]
    [SerializeField] private SlimeType slimeType;
    [SerializeField] private int slimeToCreate;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private Vector2 minCreationVelocity;
    [SerializeField] private Vector2 maxCreationVelocity;

    [Header("Attack info")]
    public float playerDistence;
    [SerializeField] private Vector2 playerCheck;
    [SerializeField] public float attackCooldown;
    [SerializeField] public float minAttackCooldown;
    [SerializeField] public float maxAttackCooldown;
    [HideInInspector] public float lastAttackTime;
    public float battleTime;

    public Vector2[] attackMove;

    public SlimeIdelState idelState { get; private set; }
    public SlimeMoveState moveState { get; private set; }
    public SlimeBattleState battleState { get; private set; }
    public SlimeAttackState attackState { get; private set; }
    public SlimeStunnedState stunnedState { get; private set; }
    public SlimeDeathState deathState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        idelState = new SlimeIdelState(stateMachine, this, "idel", this);
        moveState = new SlimeMoveState(stateMachine, this, "move", this);
        battleState = new SlimeBattleState(stateMachine, this, "move", this);
        attackState = new SlimeAttackState(stateMachine, this, "attack", this);
        stunnedState = new SlimeStunnedState(stateMachine, this, "stun", this);
        deathState = new SlimeDeathState(stateMachine, this, "idel", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idelState);
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.D))
            stateMachine.ChangeState(stunnedState);
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deathState);

        if (slimeType == SlimeType.small) return;
        else
        {
            CreateSlime(slimeToCreate, slimePrefab);
        }
    }

    private void CreateSlime(int _amount, GameObject _slimePrefab)
    {
        for(int i = 0; i < _amount; i++)
        {
            GameObject newSlime = Instantiate(_slimePrefab, transform.position, Quaternion.identity);
            newSlime.transform.Rotate(0, 180, 0);
            newSlime.GetComponent<Slime_Enemy>().SetupSlime(faceDir);
        }
    }

    public void SetupSlime(int _faceDir)
    {
        
        //Flip();
        float xVelocity = Random.Range(minCreationVelocity.x, maxCreationVelocity.x);
        float yVelocity = Random.Range(minCreationVelocity.y, maxCreationVelocity.y);
        isHitBack = true;
        GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * -faceDir, yVelocity);
        Invoke("CancelHitBack", 1.5f);
    }

    private void CancelHitBack() => isHitBack = false;

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, playerCheck * faceDir);

    }
}
