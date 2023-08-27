using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStarts : CharacterStarts
{
    private Enemy enemy;
    protected override void Start()
    {
        base.Start();
        enemy = GetComponent<Enemy>();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        enemy.DamageEffect();
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();
    }


}
