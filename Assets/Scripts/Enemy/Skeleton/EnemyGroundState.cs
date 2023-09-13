using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundState : EnemyState
{
    protected Skeleton_Enemy enemy;
    private Transform player;
    public EnemyGroundState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Skeleton_Enemy enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Updata()
    {
        base.Updata();
        if(enemy.isPlayerDetect() || Vector2.Distance(player.transform.position, enemy.transform.position) < enemy.agroDis)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
