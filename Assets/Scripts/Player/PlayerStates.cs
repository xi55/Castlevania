using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates
{
    protected PlayerStateMachine stateMachine { get; private set; }
    protected Player player;
    protected Rigidbody2D rb;
    protected float stateTime;
    protected bool triggerCalled;
    protected float xInput, yInput;
    private string animBoolName;
    

    public PlayerStates(PlayerStateMachine stateMachine, Player player, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.player = player;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        player.animator.SetBool(animBoolName, true);
        rb = player.rb;
        triggerCalled = false;
        
    }

    public virtual void Update()
    {
        stateTime -= Time.deltaTime;
        xInput = Input.GetAxis("Horizontal");
        player.animator.SetFloat("Yvelocity", rb.velocity.y);
    }

    public virtual void Exit()
    {
        player.animator.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

}
