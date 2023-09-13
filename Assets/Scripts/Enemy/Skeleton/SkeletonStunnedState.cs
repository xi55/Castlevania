using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    protected Skeleton_Enemy enemy;
    private Transform player;
    public SkeletonStunnedState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Skeleton_Enemy enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.stunnedDuration;
        rb.velocity = new Vector2(enemy.stunnedDirection.x * -enemy.faceDir, enemy.stunnedDirection.y);

        enemy.fx.InvokeRepeating("RedColorBlink", 0, 0.1f);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.fx.Invoke("CancelColorChange", 0);
    }

    public override void Updata()
    {
        base.Updata();
        if(stateTimer < 0)
            stateMachine.ChangeState(enemy.idelState);
    }
}
