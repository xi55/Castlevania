using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerStates
{
    public int comboCounter { get; private set; }
    private float comboTimer = 0.8f;
    private float lastTimeAttack;

    public PlayerPrimaryAttackState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        

        if(comboCounter > 2 || Time.time > lastTimeAttack + comboTimer)
            comboCounter = 0;

        player.animator.SetInteger("comboCount", comboCounter);
        float attackDir = player.faceDir;

        if (xInput != 0)
            attackDir = xInput;
        player.SetVelocity(player.attackMove[comboCounter].x * attackDir, player.attackMove[comboCounter].y * attackDir);
    }

    public override void Exit()
    {
        base.Exit();
        comboCounter++;
        lastTimeAttack = Time.time;
        stateTime = .1f;
        player.StartCoroutine(player.IsBusy(.15f));

    }

    public override void Update()
    {
        base.Update();

        if (stateTime < 0)
            player.SetVelocity(0, 0);

        if(triggerCalled)
            stateMachine.ChangeState(player.IdelState);
    }
}
