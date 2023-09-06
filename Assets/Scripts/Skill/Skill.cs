using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float cooldown;
    [SerializeField] protected float cooldownTimer;
    [SerializeField] protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
        CheckUnlock();
    }
    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;

    }

    public virtual bool CanUseSkill()
    {
        if(cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }
        Debug.Log("cool down");
        return false; 
    }

    public virtual void UseSkill()
    {

    }

    protected virtual void CheckUnlock()
    {

    }

    protected virtual Transform GetCloseTarget(Transform _checkTransform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 10);
        float minClosesTarget = Mathf.Infinity;
        Transform closeTarget = null;
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (Vector2.Distance(_checkTransform.position, hit.transform.position) < minClosesTarget)
                {
                    minClosesTarget = Vector2.Distance(_checkTransform.position, hit.transform.position);
                    closeTarget = hit.transform;
                }
            }
        }
        return closeTarget;
    }
}
