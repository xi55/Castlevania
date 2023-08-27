using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleState : PlayerStates
{
    private float flyTime = .4f;
    private bool skillUsed;
    private float defultGravity;

    public PlayerBlackHoleState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        skillUsed = false;
        stateTime = flyTime;
        defultGravity = rb.gravityScale;
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = defultGravity;
        player.MakeTransprent(false);
    }

    public override void Update()
    {
        base.Update();

        if (stateTime > 0)
            rb.velocity = new Vector2(0, 15f);
        if (stateTime < 0)
        {
            rb.velocity = new Vector2(0, -0.1f);
            if(!skillUsed)
            {
                if(player.skill.blackHoleSkill.CanUseSkill())
                    skillUsed = true;
            }
        }
        if(player.skill.blackHoleSkill.BlackHoleFinished())
            stateMachine.ChangeState(player.airState);
    }

    
}
