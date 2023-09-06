using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Effect", menuName = "Data/Item Effect/Ice And Fire Effect")]
public class IceAndFire_Effect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] private Vector2 velocity;

    public override void ExecuteEffect(Transform _enemyPos)
    {
        //base.ExecuteEffect(_enemyPos);
        Player player = PlayerManager.instance.player;
        //Debug.Log(player.primaryAttackState.comboCounter);
        bool thirdAttack = player.primaryAttackState.comboCounter == 2;
        if (thirdAttack)
        {
            GameObject newIceAndEffect = Instantiate(iceAndFirePrefab, _enemyPos.position, player.transform.rotation);
            newIceAndEffect.GetComponent<Rigidbody2D>().velocity = velocity * player.faceDir;
            Destroy(newIceAndEffect, 5f);
        }

        
    }
}
