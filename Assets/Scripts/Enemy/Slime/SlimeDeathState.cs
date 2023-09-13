using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDeathState : EnemyState
{
    protected Slime_Enemy enemy;
    public SlimeDeathState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Slime_Enemy _enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.animator.SetBool(enemy.lastAnimBoolName, true);
        enemy.animator.speed = 0;
        enemy.col.enabled = false;

        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Updata()
    {
        base.Updata();
        if (stateTimer > 0)
            rb.velocity = new Vector2(0, 10);
    }
}
