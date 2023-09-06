using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Freeze Effect", menuName = "Data/Item Effect/Freeze Effect")]
public class FreezeEnemy_Effect : ItemEffect
{
    [SerializeField] private float freezeDuration;

    public override void ExecuteEffect(Transform _enemyPos)
    {
        PlayerStarts playerStarts = PlayerManager.instance.player.GetComponent<PlayerStarts>();
        //base.ExecuteEffect(_enemyPos);
        if(Inventory.instance.CanUseArmor())
        {
            if (playerStarts.currentHealth > playerStarts.GetMaxHealthValue() * .2f) return;
            Collider2D[] collider = Physics2D.OverlapCircleAll(_enemyPos.position, 2f);
            foreach (Collider2D hit in collider)
            {
                if (hit.GetComponent<Enemy>() != null)
                {
                    hit.GetComponent<Enemy>().FreezeEnemy(freezeDuration);
                }
            }
        }
    }
}
