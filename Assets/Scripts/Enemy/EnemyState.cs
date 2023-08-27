using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine { get; private set; }
    protected Enemy enemyBase;
    protected string animBoolName;
    protected float stateTimer;
    protected Rigidbody2D rb;
    protected bool triggerCalled;
    public EnemyState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.enemyBase = enemyBase;
        this.animBoolName = animBoolName;
        
    }

    public virtual void Enter()
    {
        enemyBase.animator.SetBool(animBoolName, true);
        rb = enemyBase.rb;
        triggerCalled = false;
    }
    public virtual void Exit()
    {
        enemyBase.animator.SetBool(animBoolName, false);
        enemyBase.SetLastAnimBoolName(animBoolName);
    }

    public virtual void Updata()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }


}
