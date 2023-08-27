using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStarts : CharacterStarts
{
    private Player player;

    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        player.DamageEffect();
    }

    protected override void Die()
    {
        base.Die();
        player.Die();
    }

}
