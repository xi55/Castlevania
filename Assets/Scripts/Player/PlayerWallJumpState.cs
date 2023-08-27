using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerStates
{
    public PlayerWallJumpState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTime = .4f;
        player.SetVelocity(5 * -player.faceDir, player.JumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTime < 0)
            stateMachine.ChangeState(player.airState);

        if(player.IsGroundDetect())
            stateMachine.ChangeState(player.IdelState);

    }
}
