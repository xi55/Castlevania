using UnityEngine;
public class SkeletonAttackState : EnemyState
{
    protected Skeleton_Enemy enemy;
    public SkeletonAttackState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Skeleton_Enemy enemy) : base(stateMachine, enemyBase, animBoolName)
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
        if(triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }
}