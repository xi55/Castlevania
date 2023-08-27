public class SkeletonMoveState : EnemyGroundState
{
    public SkeletonMoveState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Skeleton_Enemy enemy) : base(stateMachine, enemyBase, animBoolName, enemy)
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
            stateMachine.ChangeState(enemy.IdelState);
        }
    }
}