using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public PlayerStateMachine stateMachine { get; private set; }


    [Header("Move info")]
    public float moveSpeed;
    public float jumpForce;
    public float returnImpact;
    private float defultMoveSpeed;
    private float defultJumpForce;
    

    [Header("Dash info")]
    public float dashSpeed;
    public float dashDir;
    public float defultDashSpeed;

    [Header("CounterAttack info")]
    public float counterAttackDuration;

    public Vector2[] attackMove;
    [HideInInspector]public SkillManager skill;

    public GameObject sword;


    public PlayerIdelState IdelState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerBlackHoleState blackHoleState { get; private set; }
    public PlayerDeathState deathState { get; private set; }
    // Start is called before the first frame update

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        IdelState = new PlayerIdelState(stateMachine, this, "idel");
        moveState = new PlayerMoveState(stateMachine, this, "move");
        jumpState = new PlayerJumpState(stateMachine, this, "jump");
        airState = new PlayerAirState(stateMachine, this, "jump");
        dashState = new PlayerDashState(stateMachine, this, "dash");
        wallSlideState = new PlayerWallSlideState(stateMachine, this, "wallSlide");
        wallJumpState = new PlayerWallJumpState(stateMachine, this, "jump");
        primaryAttackState = new PlayerPrimaryAttackState(stateMachine, this, "attack");
        counterAttackState = new PlayerCounterAttackState(stateMachine, this, "counterAttack");
        aimSwordState = new PlayerAimSwordState(stateMachine, this, "aimSword");
        catchSwordState = new PlayerCatchSwordState(stateMachine, this, "catchSword");
        blackHoleState = new PlayerBlackHoleState(stateMachine, this, "jump");
        deathState = new PlayerDeathState(stateMachine, this, "death");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(IdelState);
        skill = SkillManager.instance;

        defultMoveSpeed = moveSpeed;
        defultJumpForce = jumpForce;
        defultDashSpeed = dashSpeed;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        Dash();

        if(Input.GetKeyDown(KeyCode.S) && skill.crystalSkill.crystalUnlocked)
            skill.crystalSkill.CanUseSkill();

        if (Input.GetKeyDown(KeyCode.Alpha1))
            Inventory.instance.UseFlask();
    }

    public override void SlowCharacter(float _slowPercentage, float _slowDuration)
    {
        moveSpeed *= 1 - _slowPercentage;
        jumpForce *= 1 - _slowPercentage;
        dashSpeed *= 1 - _slowPercentage;

        Invoke("ReturnDefultSpeed", _slowDuration);
    }

    protected override void ReturnDefultSpeed()
    {
        base.ReturnDefultSpeed();

        moveSpeed = defultMoveSpeed;
        jumpForce = defultJumpForce;
        dashSpeed = defultDashSpeed;
    }

    public void Dash()
    {
        if (IsWallDetect())
            return;

        if (skill.dashSkill.dashUnlocked == false) return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dashSkill.CanUseSkill())
        {
            dashDir = Input.GetAxis("Horizontal");
            if (dashDir == 0)
                dashDir = faceDir;
            stateMachine.ChangeState(dashState);
        }
    }

    public void AssignSword(GameObject _sword)
    {
        sword = _sword;
    }
    public void ClearSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
        sword = null;
    }

    public void ExitBlackHoleState()
    {
        stateMachine.ChangeState(airState);
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deathState);
    }

}
