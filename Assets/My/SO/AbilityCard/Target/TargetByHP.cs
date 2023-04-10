using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Target_Abilties", menuName = "Abilties/Targets/ByHP")]
public class TargetByHP : Ability_Target
{
    [SerializeField] bool isAscendingOrder;

    public override Transform Active(List<Transform> enemyList)
    {
        Transform target = enemyList[0];
        int baseHP = enemyList[0].gameObject.GetComponent<Health>().GetHP();
        int tempHP = baseHP;

        foreach(var enemy in enemyList)
        {
            int enemyHP = enemy.gameObject.GetComponent<Health>().GetHP();

            tempHP = HPSetting(isAscendingOrder, baseHP, enemyHP);

            if (tempHP != baseHP) target = enemy.transform;
        }

        return target;
    }

    int HPSetting(bool isAscendingOrder, int baseHP, int enemyHP)
    {
        int hp = 0;

        if (isAscendingOrder) hp = baseHP > enemyHP ? enemyHP : baseHP;
        else hp = hp = baseHP < enemyHP ? enemyHP : baseHP;

        return hp;
    }
}
