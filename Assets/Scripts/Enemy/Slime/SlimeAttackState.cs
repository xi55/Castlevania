using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttackState : EnemyState
{
    protected Slime_Enemy enemy;
    public SlimeAttackState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Slime_Enemy enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastAttackTime = Time.time;
    }

    public override void Updata()
    {
        base.Updata();
        enemy.SetVelocity(0, 0);
        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }
}
