using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerStates
{
    private bool canCreateClone;

    public PlayerCounterAttackState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        canCreateClone = true;
        stateTime = player.counterAttackDuration;
        player.animator.SetBool("successfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRedius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if(hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTime = 10f;
                    player.animator.SetBool("successfulCounterAttack", true);
                    player.skill.parrySkill.UseSkill();

                    if(canCreateClone)
                    {
                        canCreateClone = false;
                        player.skill.parrySkill.MakeParryWithMirage(hit.transform);
                    }
                }
            }
        }
        if(stateTime < 0 || triggerCalled)
            stateMachine.ChangeState(player.IdelState);
    }
}
