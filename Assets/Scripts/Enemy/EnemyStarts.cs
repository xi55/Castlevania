using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStarts : CharacterStarts
{
    private Enemy enemy;
    private ItemDrop itemDrop;

    [Header("Level details")]
    [SerializeField] private int level;

    [Range(0f, 1f)]
    [SerializeField] private float percantageModifier;

    protected override void Start()
    {
        AddLevelModify();
        base.Start();
        enemy = GetComponent<Enemy>();
        itemDrop = GetComponent<ItemDrop>();
    }

    private void AddLevelModify()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);
        Modify(critChance);
        Modify(iceDamage);

        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightingDamage);
    }

    protected override void Update()
    {
        base.Update();
    }

    private void Modify(Start _start)
    {
        for(int i = 0; i < level; i++)
        {
            float modifier = _start.GetValue() * percantageModifier;
            _start.AddModifiers(Mathf.RoundToInt(modifier));
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();
        itemDrop.GenerateDrop();
    }


}
