using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public EnemyStateMachine stateMachine;
    [SerializeField] private LayerMask isPlayer;
    [Header("Stunned info")]
    public float stunnedDuration;
    public Vector2 stunnedDirection;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;

    [Header("Move info")]
    [SerializeField] public float moveSpeed;
    [SerializeField] public float idelTime;
    private float defultSpeed;

    [HideInInspector] public CharacterFX fx;

    public string lastAnimBoolName { get; private set; }
 

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        defultSpeed = moveSpeed;
    }

    protected override void Start()
    {
        base.Start();
        fx = GetComponent<CharacterFX>();
    }

    // Update is called once per frame
    protected override  void Update()
    {
        base.Update();
        stateMachine.currentState.Updata();
    }

    public void SetLastAnimBoolName(string _lastAnimBoolName)
    {
        lastAnimBoolName = _lastAnimBoolName;
    }
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned=false;
        counterImage.SetActive(false);
    }

    public virtual bool CanBeStunned()
    {
        if(canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

    public virtual void FreezeTime(bool _isTimeFreeze)
    {
        if(_isTimeFreeze)
        {
            animator.speed = 0;
            moveSpeed = 0;
        }
        else
        {
            animator.speed = 1;
            moveSpeed = defultSpeed;
        }
    }

    public virtual IEnumerator FreezeTimeFor(float _freezeTime)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_freezeTime);
        FreezeTime(false);
    }

    public virtual void FreezeEnemy(float _freezeTime) => StartCoroutine(FreezeTimeFor(_freezeTime));
    public virtual RaycastHit2D isPlayerDetect() => Physics2D.Raycast(this.transform.position, Vector2.right * faceDir, 15, isPlayer);
    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}
