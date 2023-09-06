using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerStates
{
    public PlayerDashState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTime = 0.15f;
        player.skill.dashSkill.CloneOnDash();
    }

    public override void Exit()
    {
        base.Exit();
        player.skill.dashSkill.CloneOnArrival();
    }

    public override void Update()
    {
        base.Update();
        if(!player.IsGroundDetect() && player.IsWallDetect())
            stateMachine.ChangeState(player.wallSlideState);

        player.SetVelocity(player.faceDir * player.dashSpeed, 0);

        if(stateTime < 0)
        {
            stateMachine.ChangeState(player.IdelState);
        }
    }
}
