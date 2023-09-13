using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeIdelState : SlimeGroundState
{
    public SlimeIdelState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Slime_Enemy enemy) : base(stateMachine, enemyBase, animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idelTime;
        enemy.SetVelocity(0, 0);
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.PlaySfx(24, enemy.transform);
    }

    public override void Updata()
    {
        base.Updata();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.moveState);


    }
}
