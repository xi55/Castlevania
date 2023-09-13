using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMoveState : SlimeGroundState
{
    public SlimeMoveState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Slime_Enemy enemy) : base(stateMachine, enemyBase, animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Updata()
    {
        base.Updata();
        enemy.SetVelocity(enemy.faceDir * enemy.moveSpeed, rb.velocity.y);
        if (!enemy.IsGroundDetect() || enemy.IsWallDetect())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idelState);
        }
    }
}
