using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(0, 0);
        AudioManager.instance.PlaySfx(14, null);
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0, 0);
        AudioManager.instance.StopSfx(14);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);

        if (xInput == 0 || player.IsWallDetect())
        {
            stateMachine.ChangeState(player.IdelState);
        }
    }
}
