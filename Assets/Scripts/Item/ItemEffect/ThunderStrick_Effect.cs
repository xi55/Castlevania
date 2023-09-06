using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New strick effect", menuName = "Data/Item Effect/Thunder Strick")]
public class ThunderStrick_Effect : ItemEffect
{
    [SerializeField] private GameObject thunderPrefab;
    public override void ExecuteEffect(Transform _enemyPos)
    {
        GameObject newThunder = Instantiate(thunderPrefab, _enemyPos.position, Quaternion.identity);
        Destroy(newThunder, 0.5f);
    }
}
