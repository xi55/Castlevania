using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrick_Controller : MonoBehaviour
{


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            PlayerStarts playerStarts = PlayerManager.instance.player.GetComponent<PlayerStarts>();
            EnemyStarts enemyStarts = collision.GetComponent<EnemyStarts>();
            playerStarts.DoMagicDamage(enemyStarts);
        }
    }
}
