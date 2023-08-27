using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerStates
{
    public PlayerAimSwordState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        player.skill.swordSkill.DotsActive(true);

        player.StartCoroutine(player.IsBusy(0.1f));
    }

    public override void Exit()
    {
        base.Exit();
        
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(0, 0);
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            stateMachine.ChangeState(player.IdelState);
        }
    }
}
