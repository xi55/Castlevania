using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Enemy : Enemy
{
    [Header("Attack info")]
    public float playerDistence;
    [SerializeField] private Vector2 playerCheck;
    [SerializeField] public float attackCooldown;
    [SerializeField] public float minAttackCooldown;
    [SerializeField] public float maxAttackCooldown;
    [HideInInspector] public float lastAttackTime;
    public float battleTime;

    

    public Vector2[] attackMove;

    public SkeletonIdelState idelState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    public SkeletonStunnedState stunnedState { get; private set; }
    public EnemyDeathState deathState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        idelState = new SkeletonIdelState(stateMachine, this, "idel", this);
        moveState = new SkeletonMoveState(stateMachine, this, "move", this);
        battleState = new SkeletonBattleState(stateMachine, this, "move", this);
        attackState = new SkeletonAttackState(stateMachine, this, "attack", this);
        stunnedState = new SkeletonStunnedState(stateMachine, this, "stuned", this);
        deathState = new EnemyDeathState(stateMachine, this, "idel", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idelState);
    }

    protected override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.D))
            stateMachine.ChangeState(stunnedState);
    }

    public override bool CanBeStunned()
    {
        if(base.CanBeStunned())
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

    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, playerCheck * faceDir);
        
    }
}
