using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New strick effect", menuName = "Data/Item Effect/Heal")]
public class Hea_lEffect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] float healPrecent;
    public override void ExecuteEffect(Transform _enemyPos)
    {
        //base.ExecuteEffect(_enemyPos);
        PlayerStarts playerStarts = PlayerManager.instance.player.GetComponent<PlayerStarts>();

        int healAmount = Mathf.RoundToInt(playerStarts.maxHealth.GetValue() * healPrecent);

        playerStarts.IncreaseHealth(healAmount);
    }
}
