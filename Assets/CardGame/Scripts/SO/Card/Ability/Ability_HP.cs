using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Abilties", menuName = "Abilties/Buffs/HP")]
public class Ability_HP : Ability_Origin
{
    [SerializeField] int value = 0;

    public override void Active(ICardTargetable target)
    {
        target.HPControl(value);
    }
}
