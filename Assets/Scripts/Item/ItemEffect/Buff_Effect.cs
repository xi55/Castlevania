using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New buff effect", menuName = "Data/Item Effect/Buff Effect")]
public class Buff_Effect : ItemEffect
{
    private PlayerStarts playerStarts;
    [SerializeField] private int buffAmount;
    [SerializeField] private float buffDuration;
    [SerializeField] private StatType buffType;

    public override void ExecuteEffect(Transform _enemyPos)
    {
        //base.ExecuteEffect(_enemyPos);
        playerStarts = PlayerManager.instance.player.GetComponent<PlayerStarts>();
        playerStarts.IncreasesStat(buffAmount, buffDuration, playerStarts.GetStat(buffType));
    }
}


