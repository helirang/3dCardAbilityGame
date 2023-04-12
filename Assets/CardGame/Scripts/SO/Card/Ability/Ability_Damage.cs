using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Abilties", menuName = "Abilties/Buffs/Damage")]
public class Ability_Damage : Ability_Origin
{
    [SerializeField] int damageValue = 0;

    public override void Active(ICardTargetable target)
    {
        target.DamageControl(damageValue);
    }
}
