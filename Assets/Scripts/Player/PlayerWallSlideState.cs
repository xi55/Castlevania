using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerStates
{
    public PlayerWallSlideState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
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

    public override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }

        int inputDir = (int)(xInput >= 0 ? Mathf.Ceil(xInput) : Mathf.Floor(xInput));
        
        if(!player.IsWallDetect())
            stateMachine.ChangeState(player.IdelState);

        if (xInput != 0 && player.faceDir != inputDir)
        {
            stateMachine.ChangeState(player.airState);
        }
        player.SetVelocity(0, rb.velocity.y * 0.8f);

        if (player.IsGroundDetect())
            stateMachine.ChangeState(player.IdelState);

    }

}
