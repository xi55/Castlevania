using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerStates
{
    public PlayerGroundedState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
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

        if(Input.GetKeyDown(KeyCode.F) && player.skill.blackHoleSkill.blackHoleUnlocked)
            stateMachine.ChangeState(player.blackHoleState);

        if(Input.GetKeyDown(KeyCode.LeftControl) && HasNoSword() && player.skill.swordSkill.swordUnlock)
            stateMachine.ChangeState(player.aimSwordState);

        if (Input.GetKeyDown(KeyCode.C) && player.skill.parrySkill.parryUnlocked)
            stateMachine.ChangeState(player.counterAttackState);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            stateMachine.ChangeState(player.primaryAttackState);
        }

        if(!player.IsGroundDetect())
            stateMachine.ChangeState(player.airState);

        if(Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetect())
            stateMachine.ChangeState(player.jumpState);
    }

    private bool HasNoSword()
    {
        if (!player.sword) return true;
        player.sword.GetComponent<Sword_Controller>().ReturnSword();
        return false;
    }
}
