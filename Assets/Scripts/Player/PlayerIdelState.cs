using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdelState : PlayerGroundedState
{
    public PlayerIdelState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(0, 0);
    }

    public override void Exit()
    {
        base.Exit();
        
    }

    public override void Update()
    {
        base.Update();
        int inputDir = (int)(xInput >= 0 ? Mathf.Ceil(xInput) : Mathf.Floor(xInput));
        if (inputDir == player.faceDir && player.IsWallDetect())
            return;

        if (xInput != 0 && player.IsGroundDetect() && !player.isBusy)
            stateMachine.ChangeState(player.moveState);
    }
}
