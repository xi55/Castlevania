using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerStates
{
    private Transform sword;
    public PlayerCatchSwordState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        sword = player.sword.transform;
        player.fx.playDustFx();
        player.fx.ScreenShack();
        if (player.transform.position.x > sword.position.x && player.faceDir == 1)
            player.Flip();
        else if (player.transform.position.x < sword.position.x && player.faceDir == -1)
            player.Flip();

        rb.velocity = new Vector2(player.returnImpact * -player.faceDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine(player.IsBusy(0.15f));
    }

    public override void Update()
    {
        base.Update();

        if(triggerCalled)
            stateMachine.ChangeState(player.IdelState);
    }
}
