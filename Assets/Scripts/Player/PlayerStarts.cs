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
        //player.DamageEffect();
    }

    protected override void Die()
    {
        base.Die();
        player.Die();

        GameManager.instance.lostCurrencyAmount = PlayerManager.instance.currency;
        PlayerManager.instance.currency = 0;

        GetComponent<PlayerDropItem>()?.GenerateDrop();

    }

    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);

        ItemData_Equipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);
        if (currentArmor != null)
            currentArmor.ItemEffect(transform);
    }

    public override void OnEvasion()
    {
        base.OnEvasion();
        player.skill.dodgeSkill.CreateMirageOnDodge();
    }

    public void CloneDoDamage(CharacterStarts _target, float _multipler)
    {
        if (TargetCanAvoidAttack(_target))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();
        if(_multipler > 0)
            totalDamage = Mathf.RoundToInt(totalDamage * _multipler);
        if (CanCrit())
            totalDamage = ClaculateCriticalDamage(totalDamage);

        totalDamage = CheckTargetArmor(_target, totalDamage);

        _target.TakeDamage(totalDamage);
    }

}
