using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability_Origin : ScriptableObject
{
    [Tooltip("True면 어빌리티 즉시 발동, False면 캐릭터가 공격하는 시점에 발동")]
    [SerializeField] bool isDirectAbility = false;

    public abstract void Active(ICardTargetable target);

    public bool GetIsDirect()
    {
        return isDirectAbility;
    }
}
