using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private Player player;
    
    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void AnimationCalled()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRedius);
        foreach(var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                //hit.GetComponent<Enemy>().Damage();
                EnemyStarts enemy = hit.GetComponent<EnemyStarts>();
                if(enemy != null)
                    player.states.DoDamage(enemy);
                ItemData_Equipment weaponData =  Inventory.instance.GetEquipment(EquipmentType.Weapon);
                if (weaponData != null)
                    weaponData.ItemEffect(enemy.transform);
            }
        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.swordSkill.CreateSword(player);
    }
  
}
